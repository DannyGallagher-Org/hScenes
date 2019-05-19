using UnityEditor;

namespace AssetBundles
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CreateAssetBundles
    {
        [MenuItem("How/AssetBundles/Build AssetBundles PC")]
        // ReSharper disable once UnusedMember.Local
        public static void BuildAllAssetBundlesPc()
        {
            BuildPipeline.BuildAssetBundles(
UnityEngine.Application.streamingAssetsPath + "/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
        
        [MenuItem("How/AssetBundles/Build AssetBundles MAC")]
        // ReSharper disable once UnusedMember.Local
        public static void BuildAllAssetBundlesMac()
        {
            BuildPipeline.BuildAssetBundles(
                UnityEngine.Application.streamingAssetsPath + "/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
        }

        [MenuItem("How/AssetBundles/Get AssetBundle names")]
        // ReSharper disable once UnusedMember.Local
        public static void GetNames()
        {
            var names = AssetDatabase.GetAllAssetBundleNames();
            foreach (var name in names)
                UnityEngine.Debug.Log("AssetBundle: " + name);
        }
    }
}