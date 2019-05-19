using System.Collections.Generic;
using UnityEngine;

namespace GenericDataSet
{
    [CreateAssetMenu(fileName = "NewRTDataSet", menuName = "How/RT Data Set")]
    public class RuntimeDataSet<T> : ScriptableObject {

        #region Interface
        private List<T> _items = new List<T>();

        public List<T> Items => _items;

        public int Count => _items.Count;
        #endregion

        #region Public Methods
        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Remove(T item)
        {
            _items.Remove(item);
        }
        #endregion
    }
}