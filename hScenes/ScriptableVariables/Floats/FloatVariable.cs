using UnityEngine;

namespace ScriptableVariables.Floats
{
    [CreateAssetMenu(fileName = "NewFloatVar", menuName = "DataVariables/Float")]
    public class FloatVariable : ScriptableObject
    {
        public float Value;
    }
}