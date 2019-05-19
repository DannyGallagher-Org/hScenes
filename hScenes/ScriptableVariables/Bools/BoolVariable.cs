using UnityEngine;

namespace ScriptableVariables.Bools
{
    [CreateAssetMenu(fileName = "NewBoolVar", menuName = "DataVariables/Bool")]
    public class BoolVariable : ScriptableObject
    {
        public bool Value;
    }
}