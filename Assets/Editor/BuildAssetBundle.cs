using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using System.IO;

public class BuildAssetBundle
{
    [MenuItem("Assets/Build All Asset Bundles")]
    static void BuildAllAssetBundles()
    {
        string directory = "Assets/StreamingAssets/AssetBundles";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.iOS);

        /*

#if UNITY_ANDROID
        BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.Android);
#elif UNITY_EDITOR || UNITY_WIN
        BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
#elif UNITY_IOS
        BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.iOS);
#elif UNITY_STANDALONE_OSX
        BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
#endif
        */
    }
}
