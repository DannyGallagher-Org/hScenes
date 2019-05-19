using System;
using ScriptableVariables.Strings;
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
        public StringReference AssetBundle = new StringReference();

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

        public void AddOptionalLayer(string mission, string layer)
        {
            try
            {
                if (_optionalLayers.ContainsKey(mission))
                {
                    _optionalLayers[mission] += $";{layer}";
                    return;
                }
                
                _optionalLayers.Add(mission, layer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}