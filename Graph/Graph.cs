using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    public interface IGraph<T>
    {
        IObservable<IEnumerable<T>> RoutesBetween(T source, T target);
    }

    public class Graph<T> : IGraph<T>
    {
        private readonly IEnumerable<ILink<T>> _links;

        public Graph(IEnumerable<ILink<T>> links)
        {
            _links = links;
        }

        public IObservable<IEnumerable<T>> RoutesBetween(T source, T target)
        {
            List<T> lettersAlreadyUsed = new List<T>();
            List<ILink<T>> NewListLinks = new List<ILink<T>>();
            while (_links.Except(NewListLinks).Any(x => x.Source.Equals(source)))
            {
                var s = source;
                foreach (var link in _links)
                {
                    if (link.Source.Equals(s) && !lettersAlreadyUsed.Contains(link.Target))
                    {
                        lettersAlreadyUsed.Add(link.Source);
                        s = link.Target;
                        NewListLinks.Add(link);
                    }
                }
            }

            return null;
        }
    }
}
