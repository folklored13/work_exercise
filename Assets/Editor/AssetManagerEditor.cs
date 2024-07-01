using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public enum AssetBundleCompressionPattern
{
    LZMA,
    LZ4,
    None
}

/// <summary>
/// 所有在Editor目录下的C#脚本都不会跟着资源打包到可执行文件包体中
/// </summary>
public class AssetManagerEditor 
{
    //声明版本号
    public static string AssetManagerVersion = "1.0.0";

    /// <summary>
    /// 编辑器模拟下，不进行打包
    /// 本地模式，打包到StreamingAssets
    /// 远端模式，打包到任意远端路径，在该示例中为persistentDataPath
    /// </summary>
    public static AssetBundlePattern BuildingPattern;

    public static AssetBundleCompressionPattern CompressionPattern;

    /// <summary>
    /// 需要打包的文件夹
    /// </summary>
    public static DefaultAsset AssetBundleDirectory;



    public static string AssetBundleOutputPath;

    /// <summary>
    /// 通过MenuItem特性，声明Editor顶部菜单栏选项
    /// </summary>
    [MenuItem(nameof(AssetManagerEditor)+"/"+nameof(BuildAssetBundle))]
    static void BuildAssetBundle()
    {
        CheckBuildOutputPath();
        //PathCombine方法可以在几个字符串之间插入斜杠
        //string outputPath = "E:/AssetBundles/testAB1";
        //string outputPath = "E:/AssetBundles/testAB2";
        //string outputPath = "E:/AssetBundles/testAB3";

        if (!Directory.Exists(AssetBundleOutputPath))
        {
            Directory.CreateDirectory(AssetBundleOutputPath);
        }

        //不同平台之间的AssetBundle不可以通用
        //该方法会打包工程内所有配置了包名的AB包，即如果没有设置包名就打不出包
        //Options为None时使用LZMA压缩
        //UncompressedAssetBundle不进行压缩
        //ChunkBasedCompression进行LZ4块压缩

        BuildPipeline.BuildAssetBundles(AssetBundleOutputPath, CheckCompressionPattern(), EditorUserBuildSettings.activeBuildTarget);

        Debug.Log("AB包打包已完成");
    }

    static BuildAssetBundleOptions CheckCompressionPattern()
    {
        BuildAssetBundleOptions option = new BuildAssetBundleOptions();
        switch (CompressionPattern)
        {
            case AssetBundleCompressionPattern.LZMA:
                option = BuildAssetBundleOptions.None;
                break;
            case AssetBundleCompressionPattern.LZ4:
                option = BuildAssetBundleOptions.ChunkBasedCompression;
                break;
            case AssetBundleCompressionPattern.None:
                option = BuildAssetBundleOptions.UncompressedAssetBundle;
                break;
        }
        return option;
    }
    [MenuItem(nameof(AssetManagerEditor) + "/" + nameof(OpenAssetManagerWindow))]
    static void OpenAssetManagerWindow()
    {
        //方法一,通过EditorWindow.GetWindowWithRect()获取一个具有具体矩形大小的窗口类
        //Rect windowRect = new Rect(0, 0, 500, 500);
        //AssetManagerEditorWindow window = (AssetManagerEditorWindow) EditorWindow.GetWindowWithRect(typeof
        //    (AssetManagerEditorWindow),windowRect,true,nameof(AssetManagerEditor));

        //方法二，通过EditorWindow.GetWindow()获取一个自定义大小，可任意拖拽的窗口
        //AssetManagerEditorWindow window = (AssetManagerEditorWindow)EditorWindow.GetWindow(typeof
        //    (AssetManagerEditorWindow), true, nameof(AssetManagerEditor));
        //如果不赋予名称就可以作为Unity窗口随意放置在面板中
        AssetManagerEditorWindow window = (AssetManagerEditorWindow)EditorWindow.GetWindow(typeof(AssetManagerEditorWindow));

    }

    static void CheckBuildOutputPath()
    {
        switch(BuildingPattern)
        {
            case AssetBundlePattern.EditorSimulation:
                break;
            case AssetBundlePattern.Local:
                 AssetBundleOutputPath = Path.Combine(Application.streamingAssetsPath, HelloWorld.MainAssetBundleName);
                break;
            case AssetBundlePattern.Remote:
                AssetBundleOutputPath = Path.Combine(Application.persistentDataPath, HelloWorld.MainAssetBundleName);
                break;
        }
    }

    /// <summary>
    /// 打包指定文件夹下所有资源为AssetBundle
    /// </summary>
    public static void BuildAssetBundleFromDirectory()
    {
        CheckBuildOutputPath();
        if (AssetBundleDirectory == null)
        {
            Debug.LogError("打包目录不存在");
            return;
        }
        //获取文件路径
        string directoryPath = AssetDatabase.GetAssetPath(AssetBundleDirectory);
        //将列表转化为数组
        string[] assetPaths = FindAllAssetFromDirectory(directoryPath).ToArray();

        AssetBundleBuild[] assetBundleBuild = new AssetBundleBuild[1];

        //将要打包的具体包名，而不是主包名
        assetBundleBuild[0].assetBundleName = HelloWorld.ObjectAssetBundleName;

        //这里虽然名为Name，实际上需要资源在工程下的路径
        assetBundleBuild[0].assetNames = assetPaths;

        if (string.IsNullOrEmpty(AssetBundleOutputPath))
        {
            Debug.LogError("输出路径为空");
            return;
        }
        else if (!Directory.Exists(AssetBundleOutputPath))
        {
            //若路径不存在就创建路径
            Directory.CreateDirectory(AssetBundleOutputPath);
        }
        //Unity中Inspector面板配置的AssetBundle信息，其实就是一个AssetBundle结构体
        //Unity只不过是遍历了所有的文件夹，并把我们在文件夹中配置的AssetBundle收集起来并打包
        BuildPipeline.BuildAssetBundles(AssetBundleOutputPath, assetBundleBuild, CheckCompressionPattern(),
            BuildTarget.StandaloneWindows);

        //打印输出路径
        Debug.Log(AssetBundleOutputPath);

        //刷新Project界面，如果不是打包到工程内则不需要执行
        AssetDatabase.Refresh();
    }

    public static List<string> FindAllAssetFromDirectory(string directoryPath)
    {
        List<string> assetPaths = new List<string>();
        //如果传入的路径为空或者不存在的话
        if(string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
        {
            Debug.Log("文件夹路径不存在");
            return null;
        }
        //System.IO命名空间下的类，也就是Windows自带的对文件夹进行操作的类
        //System.IO下的类，只能在PC平台或者Windows上读写文件，在移动端不适用
        DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

        //获取该目录下所有文件信息
        //Directory文件夹不属于File类型，所以这里不会获取子文件夹
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        //所有非元数据文件(后缀不是meta的文件)路径都添加到列表中用于打包这些文件
        foreach(FileInfo info in fileInfos)
        {
            //.meta文件代表描述文件
            if(info.Extension.Contains(".meta"))
            {
                continue;
            }
            //AssetBundle打包只需要文件名
            string assetPath = Path.Combine(directoryPath, info.Name);
            assetPaths.Add(assetPath);
            Debug.Log(assetPath);
        }
        return assetPaths;
    }
}