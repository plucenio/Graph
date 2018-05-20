using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

//http://rxwiki.wikidot.com/101samples

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

        public IObservable<IEnumerable<T>> RoutesBetween(T source, T target)
        {
            return Observable.Create<IEnumerable<T>>(observer =>
            {
                List<T> lettersAlreadyUsed = new List<T>();
                foreach (var item in _links.Where(x => x.Source.Equals(source)))
                {                    
                    var list = new List<T>();
                    T tempSource = item.Source;
                    while (!target.Equals(tempSource))
                    {
                        ILink<T> next = _links.Where(x => x.Source.Equals(tempSource) && !lettersAlreadyUsed.Contains(x.Target)).FirstOrDefault();
                        list.Add(tempSource);
                        lettersAlreadyUsed.Add(tempSource);
                        tempSource = next.Target;
                    }
                    list.Add(tempSource);
                    observer.OnNext(list);
                }
                observer.OnCompleted();
                return Disposable.Empty;                
            });
        }
    }
}
