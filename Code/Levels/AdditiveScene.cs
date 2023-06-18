using System;
using hUtility.ScriptableVariables.Strings;
using SerializableDictionary.Implementations;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "NewSceneObject", menuName = "How/Scene")]
    [Serializable]
    public class AdditiveScene : ScriptableObject
    {
        public StringReference Name = new StringReference();
        public StringReference LightingScene = new StringReference();

        [SerializeField]
        private SDStringString _optionalLayers;
        public SDStringString OptionalLayers => _optionalLayers;

        [SerializeField]
        [HideInInspector]
        public string[] Layers = {};

        [SerializeField]
        private SDStringString _stateLayers;
        public SDStringString StateLayers => _stateLayers;

        public AdditiveScene()
        {
            _optionalLayers = new SDStringString();
        }
    }
}