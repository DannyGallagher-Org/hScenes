using UnityEditor;
using UnityEngine;

namespace Editor.Utility
{
    public static class ScriptableObjectUtility
    {
        public static T CreateAsset<T> (string path) where T : ScriptableObject
        {
            var newObj = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(newObj, path);

            // Print the path of the created asset
            Debug.Log(AssetDatabase.GetAssetPath(newObj));
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}