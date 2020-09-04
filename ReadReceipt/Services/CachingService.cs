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
                SQLitePersistentBlobCache cache;
                try
                {
                    //Sometimes make Exception : https://github.com/reactiveui/Akavache/issues/195#issuecomment-66576874
                    cache = new SQLitePersistentBlobCache(path, BlobCache.TaskpoolScheduler);
                }
                catch
                {
                    return localCache.Value;
                }
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

        public IObservable<Unit> Remove<T>(string key)
        {
            return localCache.Value.InvalidateObject<T>(key);
        }

        public IObservable<Unit> Remove<T>(IEnumerable<string> keys)
        {
            return localCache.Value.InvalidateObjects<T>(keys);
        }

        public void ShutDown()
        {
            localCache.Value.Shutdown.Wait();
        }
    }
}
