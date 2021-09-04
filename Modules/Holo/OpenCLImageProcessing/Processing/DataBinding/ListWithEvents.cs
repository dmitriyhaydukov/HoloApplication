using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processing.DataBinding
{
    public interface IListWithEvents<T> : IList<T>
    {
        event Action<T> ItemAdded;
        event Action ItemsCleared;
        event Action<T> ItemRemoved;
        event Action ItemsAdded;
        void AddRange(IEnumerable<T> values);
    }

    public class ListWithEvents<T> : IListWithEvents<T>
    {
        private List<T> _inner;

        public event Action<T> ItemAdded;
        public event Action ItemsCleared;
        public event Action<T> ItemRemoved;
        public event Action ItemsAdded;
        /// <summary>
        /// index, old value, new value
        /// </summary>
        public event Action<int, T, T> ItemUpdated;

        public ListWithEvents()
        {
            _inner = new List<T>();
        }

        #region IList interface
        public IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _inner.Add(item);
            ItemAdded?.Invoke(item);
        }

        public void Clear()
        {
            _inner.Clear();
            ItemsCleared?.Invoke();
        }

        public bool Contains(T item)
        {
            return _inner.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            if (_inner.Remove(item))
            {
                ItemRemoved?.Invoke(item);
                return true;
            }

            return false;
        }

        public int Count => _inner.Count;
        public bool IsReadOnly => false;
        public int IndexOf(T item) => _inner.IndexOf(item);
        public void Insert(int index, T item) => _inner.Insert(index, item);
        public void RemoveAt(int index) => _inner.RemoveAt(index);

        public T this[int index]
        {
            get => _inner[index];
            set
            {
                var oldVal = _inner[index];
                _inner[index] = value;
                ItemUpdated?.Invoke(index, oldVal, value);
            }
        }
        #endregion

        public void AddRange(IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                _inner.Add(value);
            }

            ItemsAdded?.Invoke();
        }
    }
}
