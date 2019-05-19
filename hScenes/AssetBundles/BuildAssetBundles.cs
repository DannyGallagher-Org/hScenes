using System;
using System.IO;
using System.Linq;
using Levels;
using UnityEditor;
using UnityEngine;

namespace AssetBundles
{
    // ReSharper disable once UnusedMember.Global
    //[InitializeOnLoad]
    public static class BuildAssetBundles
    {
        static BuildAssetBundles()
        {
            if (Directory.GetFiles(Application.streamingAssetsPath + "/AssetBundles", "*.manifest").Any())
                return;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    BuildAllAssetBundlesMac();
                    break;
                case RuntimePlatform.OSXPlayer:
                    BuildAllAssetBundlesMac();
                    break;
                case RuntimePlatform.WindowsPlayer:
                    BuildAllAssetBundlesPc();
                    break;
                case RuntimePlatform.WindowsEditor:
                    BuildAllAssetBundlesPc();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        [MenuItem("How/AssetBundles/Build AssetBundles PC")]
        // ReSharper disable once UnusedMember.Local
        public static void BuildAllAssetBundlesPc()
        {
            BuildPipeline.BuildAssetBundles(
                Application.streamingAssetsPath + "/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
        
        [MenuItem("How/AssetBundles/Build AssetBundles MAC")]
        // ReSharper disable once UnusedMember.Local
        public static void BuildAllAssetBundlesMac()
        {
            BuildPipeline.BuildAssetBundles(
                Application.streamingAssetsPath + "/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
        }

        [MenuItem("How/AssetBundles/Get AssetBundle names")]
        // ReSharper disable once UnusedMember.Local
        public static void GetNames()
        {
            var names = AssetDatabase.GetAllAssetBundleNames();
            foreach (var name in names)
                Debug.Log("AssetBundle: " + name);
        }
        
        public static void BuildAssetBundle(AdditiveScene scene) {
            // Create the array of bundle build details.
            var buildMap = new AssetBundleBuild[1];
	
            buildMap[0].assetBundleName = scene.Name.Value;
            var names = new string[scene.Layers.Length];
            
            for (var i = 0; i < scene.Layers.Length; i++)
            {
                names[i] = $"Assets/Scenes/{scene.Name.Value}/{scene.Layers[i]}";
            }

            buildMap[0].assetNames = names;

            try
            {
                BuildPipeline.BuildAssetBundles(
                    Application.streamingAssetsPath + "/AssetBundles/scenes",
                    buildMap,
                    BuildAssetBundleOptions.None,
                    BuildTarget.StandaloneWindows);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
            finally
            {
                Debug.Log($"BuiltBundle for :{scene.Name.Value}");
            }
        }
    }
}