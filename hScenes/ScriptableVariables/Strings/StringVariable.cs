using System;
using UnityEngine;

namespace ScriptableVariables.Strings
{
    [Serializable]
    [CreateAssetMenu(fileName = "NewStringVar", menuName = "DataVariables/String")]
    public class StringVariable : ScriptableObject
    {
        public string Value;
    }
}