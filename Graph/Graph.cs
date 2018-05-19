using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Graph
{
    public interface IGraph<T> 
    {
        IObservable<IEnumerable<T>> RoutesBetween(T source, T target);
    }

    public class Graph<T> : ReactiveObject, IGraph<T>
    {
        private readonly IEnumerable<ILink<T>> _links;

        public Graph(IEnumerable<ILink<T>> links)
        {
            _links = links;
        }

        public static IObservable<IEnumerable<T>> MakeObservable_3(Action action)
        {
            return Observable.Create<IEnumerable<T>>(observer =>
            {
                try
                {
                    action();
                    observer.OnNext(new List<T>());
                    observer.OnCompleted();
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
                return Disposable.Empty;
            });
        }

        public IObservable<IEnumerable<T>> RoutesBetween(T source, T target)
        {
            return MakeObservable_3(() => {
            List<T> lettersAlreadyUsed = new List<T>();
            IEnumerable<ILink<T>> newListLinks = new List<ILink<T>>();
                while (_links.Except(newListLinks).Any(x => x.Source.Equals(source)))
                {
                    var s = source;
                    foreach (var link in _links)
                    {
                        if (link.Source.Equals(s) && !lettersAlreadyUsed.Contains(link.Target))
                        {
                            lettersAlreadyUsed.Add(link.Source);
                            s = link.Target;
                            ((List<ILink<T>>)newListLinks).Add(link);
                        }
                    }                    
                }
            });
        }
    }
}
