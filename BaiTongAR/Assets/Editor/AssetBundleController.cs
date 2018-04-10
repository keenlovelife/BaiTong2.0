using UnityEditor;
using UnityEngine;
using System.IO;
public class AssetBundleController : MonoBehaviour {

    [MenuItem("AssetBundle/开始打包")]
    static void StartBundle()
    {
       // BuildPipeline.BuildAssetBundles("Assets/StreamingAssets",BuildAssetBundleOptions.None, BuildTarget.Android);
    }
}

public class AutoGenerateAssetBundleNameImproved :AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
                                      string[] movedAssets, string[] movedFromAssetPaths)
    {
        for (int i = 0; i < importedAssets.Length;++i)
        {
            //string filepath = importedAssets[i];
            //string p = "Assets/Texture/";
            //if (filepath.Length < p.Length || filepath.Substring(0, p.Length) != p) continue;
            //if (Directory.Exists(filepath)) continue;

            //Debug.Log("ImprtedAssets：" + filepath);
            //AssetImporter importer = AssetImporter.GetAtPath(filepath);

            //string assetbundlename = filepath.Substring(p.Length);
            //Debug.Log("assetbundlename："+assetbundlename);
            //importer.assetBundleName = assetbundlename;


        }
      
    }
}