using System;
using System.Collections.Concurrent;
using System.Reactive.Subjects;

namespace RpgCombatKata.Core.Infrastructure
{
    public class EventBus : IDisposable
    {
        private readonly ConcurrentDictionary<Type, object> busDictionary = new ConcurrentDictionary<Type, object>();

        public virtual IObservable<T> Subscriber<T>()
        {
            return GetBus<T>();
        }

        public virtual void Publish<T>(T msg) {
            Console.WriteLine("Raised {0}", typeof(T).FullName);
            GetBus<T>().OnNext(msg);
        }

        private ISubject<T> GetBus<T>()
        {
            var bus = busDictionary.GetOrAdd(typeof(T), (type) => new Subject<T>());
            return bus as ISubject<T>;
        }

        public void Dispose() {
            foreach (var bus in busDictionary.Values) {
                (bus as IDisposable)?.Dispose();
            }
        }
    }
}
