using UnityEditor;
using UnityEngine;
public class CreateAssetBundle {

    [MenuItem("Create AssetBundle/Android")]
    static void CreateAssetBundle_android()
    {
        string assetBundleDirectory = "Assets/StreamingAssets";
        if (!System.IO.Directory.Exists(assetBundleDirectory))
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
            BuildAssetBundleOptions.None,
            BuildTarget.Android);
        AssetDatabase.Refresh();
    }
    [MenuItem("Create AssetBundle/iOS")]
    static void CreateAssetBundle_iOS()
    {
        string assetBundleDirectory = "Assets/StreamingAssets";
        if (!System.IO.Directory.Exists(assetBundleDirectory))
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
            BuildAssetBundleOptions.None,
            BuildTarget.iOS);
        AssetDatabase.Refresh();
    }
    [MenuItem("Create AssetBundle/Windows")]
    static void CreateAssetBundle_Windows()
    {
        string assetBundleDirectory = "Assets/StreamingAssets";
        if (!System.IO.Directory.Exists(assetBundleDirectory))
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
            BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows);
        AssetDatabase.Refresh();
    }
    [MenuItem("Create AssetBundle/Windows 64")]
    static void CreateAssetBundle_Windows_64()
    {
        string assetBundleDirectory = "Assets/StreamingAssets";
        if (!System.IO.Directory.Exists(assetBundleDirectory))
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
            BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows64);
        AssetDatabase.Refresh();
    }
    [MenuItem("Create AssetBundle/WebGL")]
    static void CreateAssetBundle_WebGL()
    {
        string assetBundleDirectory = "Assets/StreamingAssets";
        if (!System.IO.Directory.Exists(assetBundleDirectory))
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
            BuildAssetBundleOptions.None,
            BuildTarget.WebGL);
        AssetDatabase.Refresh();
    }



    static void Test()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/test/t";
        if (!System.IO.Directory.Exists(assetBundleDirectory))
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        AssetDatabase.Refresh();
    }

    static int sum = 0;
    [MenuItem("The Amount of Code/Total Code")]
    static void CodeQuantity()
    {
        string assetBundleDirectory = "D:\\ReleaseProject\\ByTabfun\\formal\\药分子式\\Pharmacy\\Assets\\Script";
        sum = 0;
        code(assetBundleDirectory);
        Debug.Log(" 总代码量：" + sum);
    }
    static int code(string folderFullName)
    {
        var theFolder = new System.IO.DirectoryInfo(folderFullName);
        var files = theFolder.GetFiles();
        foreach (var nextfile in theFolder.GetFiles())
        {
            if (nextfile.Extension == ".cs")
            {
                Debug.Log("--- 检测到程序文件：" + nextfile.FullName);
                code_quantity(nextfile.FullName);
            }
        }
        foreach (var nextfloder in theFolder.GetDirectories())
            code(nextfloder.FullName);

        return sum;
    }
    static void code_quantity(string filepath)
    {
        string line = string.Empty;
        System.IO.StreamReader file = new System.IO.StreamReader(filepath);
        int num = 0;
        while ((line = file.ReadLine()) != null)
        {
            ++sum;
            ++num;
        }
        Debug.Log("         该文件代码量：" + num);
    }

}
