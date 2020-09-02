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
        void ShutDown();
    }
}