using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        protected List<T> _items = new List<T>();

        public event Action<T> ItemAdded;
        public event Action<T> ItemRemoved;

        public void Add(T item)
        {
            _items.Add(item);

            ItemAdded?.Invoke(item);
        }

        public IReadOnlyCollection<T> Get(Func<T, bool> predicate)
        {
            return _items.Where(predicate).ToArray();
        }

        public IReadOnlyCollection<T> GetAll()
        {
            return _items.ToArray();
        }

        public void Remove(T item)
        {
            _items.Remove(item);

            ItemRemoved?.Invoke(item);
        }
    }
}