using System.Collections.Generic;
using UnityEngine;

namespace GenericDataSet
{
    [CreateAssetMenu(fileName = "NewDataSet", menuName = "How/Data Set")]
    public class DataSet : ScriptableObject
    {
        #region Interface
        public List<Object> Items = new List<Object>();

        public int Count => Items.Count;
        #endregion
    }
}