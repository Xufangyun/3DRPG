using UnityEditor;
using System.IO;


public class CreatAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAssetbundles()
    {
        string dir = "AssetBundles";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
