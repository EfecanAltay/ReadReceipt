using System;
using System.Collections.Generic;
using System.Reactive;

namespace ReadReceipt.Services
{
    public interface ICachingService
    {
        void Init();
        IObservable<T> Get<T>(string key);
        IObservable<IEnumerable<T>> GetAll<T>();
        IObservable<Unit> Set<T>(string key, T obj);
        IObservable<Unit> Remove<T>(string key);
        IObservable<Unit> Remove<T>(IEnumerable<string> keys);
        void ShutDown();
    }
}