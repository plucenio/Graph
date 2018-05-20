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
                    var ListaFinal = new List<T>();
                    T tempSource = item.Source;
                    T tempTarget = item.Target;
                    while (!target.Equals(tempSource))
                    {
                        ILink<T> next = _links.Where(x => x.Source.Equals(tempSource) && !lettersAlreadyUsed.Contains(x.Target)).FirstOrDefault();
                        ListaFinal.Add(tempSource);
                        lettersAlreadyUsed.Add(tempSource);
                        tempSource = next.Target;
                    }
                    ListaFinal.Add(tempSource);
                    observer.OnNext(ListaFinal);
                }
                observer.OnCompleted();
                return Disposable.Empty;




                /*
                var ListaFinal = new List<T>();
                foreach (var item in _links)
                {
                    T tempSource = source;
                    while (!target.Equals(tempSource))
                    {
                        ILink<T> next = _links.Where(x => x.Source.Equals(tempSource) && !ListaFinal.Contains(x.Target)).FirstOrDefault();
                        ListaFinal.Add(tempSource);                        
                        tempSource = next.Target;
                    }
                }
                observer.OnNext(ListaFinal);
                observer.OnNext(ListaFinal);
                observer.OnCompleted();
                return Disposable.Empty;
                */
            });
        }

        /*
        public IObservable<IEnumerable<T>> RoutesBetween(T source, T target)
        {
            return Observable.Create<IEnumerable<T>>(observer =>
            {
                T temp2 = target;
                foreach (var item in _links)
                {
                    List<List<ILink<T>>> listaDeListas = new List<List<ILink<T>>>();

                    List<T> lettersAlreadyUsed = new List<T>();
                    List<ILink<T>> lista = new List<ILink<T>>();
                    T tempSource = source;
                    while (!target.Equals(tempSource))
                    {                
                        ILink<T> next = _links.Except(lista).Where(x => x.Source.Equals(tempSource) && !lettersAlreadyUsed.Contains(x.Target)).FirstOrDefault();
                        lista.Add(next);
                        lettersAlreadyUsed.Add(tempSource);
                        tempSource = next.Target;                
                    }
                    listaDeListas.Add(lista);
            
                    observer.OnNext(lista);
                }
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }
        */

        #region Obsoleto

        /*
        public IObservable<IEnumerable<T>> RoutesBetween(T source, T target)
        {
            return Observable.Create<IEnumerable<T>>(observer =>
            {
                try
                {
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
        */
        #endregion
    }
}
