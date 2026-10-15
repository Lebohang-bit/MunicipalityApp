using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalityApp
{
    public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
    {
        private List<Tuple<TElement, TPriority>> items = new List<Tuple<TElement, TPriority>>();

        public int Count
        {
            get { return items.Count; }
        }

        public void Enqueue(TElement element, TPriority priority)
        {
            items.Add(new Tuple<TElement, TPriority>(element, priority));
            items = items.OrderBy(i => i.Item2).ToList();
        }

        public TElement Dequeue()
        {
            if (items.Count == 0)
                throw new InvalidOperationException("Queue is empty.");

            TElement element = items[0].Item1;
            items.RemoveAt(0);
            return element;
        }

        public TElement Peek()
        {
            if (items.Count == 0)
                throw new InvalidOperationException("Queue is empty.");

            return items[0].Item1;
        }
    }
}