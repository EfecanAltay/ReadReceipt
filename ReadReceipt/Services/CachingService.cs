using Akavache;
using Akavache.Sqlite3;
using ReadReceipt.Services;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;

[assembly: Xamarin.Forms.Dependency(typeof(CachingService))]
namespace ReadReceipt.Services
{
    public class CachingService : ICachingService
    {
        private IFilesystemProvider _filesystemProvider;
        private Lazy<IBlobCache> localCache;
        public void Init()
        {
            _filesystemProvider = Locator.Current.GetService<IFilesystemProvider>();
            BlobCache.EnsureInitialized();
            InitCaches();
        }

        private void InitCaches()
        {
            localCache = new Lazy<IBlobCache>(() =>
            {
                var cachePath = _filesystemProvider.GetDefaultLocalMachineCacheDirectory();
                _filesystemProvider.CreateRecursive(cachePath).SubscribeOn(BlobCache.TaskpoolScheduler).Wait();
                var path = Path.Combine(cachePath, "blobs.db");
                var cache = new SQLitePersistentBlobCache(path, BlobCache.TaskpoolScheduler);
                return cache;
            });
        }

        public IObservable<T> Get<T>(string key)
        {
            return localCache.Value.GetObject<T>(key);
        }

        public IObservable<IEnumerable<T>> GetAll<T>()
        {
            return localCache.Value.GetAllObjects<T>();
        }

        public IObservable<Unit> Set<T>(string key, T obj)
        {
            return localCache.Value.InsertObject<T>(key, obj);
        }

        public void ShutDown()
        {
            localCache.Value.Shutdown.Wait();
        }
    }
}
