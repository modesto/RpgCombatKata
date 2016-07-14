using System;
using System.Collections.Concurrent;
using System.Reactive.Subjects;

namespace RpgCombatKata.Core.Infrastructure
{
    public sealed class EventBus : IDisposable
    {
        private readonly ConcurrentDictionary<Type, object> busDictionary = new ConcurrentDictionary<Type, object>();

        public IObservable<T> Observable<T>()
        {
            return GetBus<T>();
        }

        public void Publish<T>(T msg) {
            GetBus<T>().OnNext(msg);
        }

        private ISubject<T> GetBus<T>()
        {
            var bus = busDictionary.GetOrAdd(typeof(T), (type) => new Subject<T>());
            return bus as ISubject<T>;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~EventBus() {
            Dispose(false);
        }


        private void Dispose(bool disposing) {
            if (disposing) {
                foreach (var bus in busDictionary.Values)
                {
                    (bus as IDisposable)?.Dispose();
                }
                busDictionary.Clear();
            }
        }
    }
}
