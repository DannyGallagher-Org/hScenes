using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static void Rearrange(this Transform a, Transform b)
        {
            a.position = b.position;
            a.rotation = b.rotation;
        } 
    }
}