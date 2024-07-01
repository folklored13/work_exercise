using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBundleVersionDifference
{
    /// <summary>
    /// 新增资源包
    /// </summary>
    public List<string> AdditionAssetBundles = new List<string>();
    /// <summary>
    /// 减少资源包
    /// </summary>
    public List<string> ReducedAssetBundles = new List<string>();
}

public enum AssetBundlePattern
{
    /// <summary>
    /// 编辑器模拟加载，应该使用AssetDataBase进行资源加载，而不用进行打包
    /// </summary>
    EditorSimulation,
    /// <summary>
    /// 本地加载模式，应打包到本地路径或StreamingAssets路径下，从该路径加载
    /// </summary>
    Local,
    /// <summary>
    /// 远端加载模式，应打包到任意资源服务器地址，然后通过网络进行下载
    /// 下载到沙盒路径persistentDataPath后，再进行加载
    /// </summary>
    Remote
}

public enum AssetBundleCompressionPattern
{
    LZMA,
    LZ4,
    None
}

/// <summary>
/// 任何BuildOption处于非forceRebuild选项下都默认为增量打包
/// </summary>
public enum IncrementalBuildMode
{
    None,
    IncrementalBuild,
    ForceRebuild
}

/// <summary>
/// Package打包后记录的信息
/// </summary>
public class PackageBuildInfo
{
    public string PackageName;
    public List<AssetBuildInfo> AssetInfos = new List<AssetBuildInfo>();
    public List<string> PackageDependencies = new List<string>();
    /// <summary>
    /// 代表是否是初始包
    /// </summary>
    public bool IsSourcePackage = false;
}

/// <summary>
/// Package中Asset打包之后记录的信息
/// </summary>
public class AssetBuildInfo
{
    /// <summary>
    /// 资源名称
    /// 当需要加载资源时，应该和字符串相同
    /// </summary>
    public string AssetName;

    /// <summary>
    /// 该资源属于哪个AssetBundle
    /// </summary>
    public string AssetBundleName;
}

public class AssetPackage
{
    public PackageBuildInfo PackageInfo;
    public string PackageName { get { return PackageInfo.PackageName; } }

    Dictionary<string, object> LoadAssets = new Dictionary<string, object>();

    public T LoadAsset<T>(string assetName) where T:Object
    {
        T assetObject = default;
        foreach(AssetBuildInfo info in PackageInfo.AssetInfos)
        {
            if(info.AssetName==assetName)
            {
                if(LoadAssets.ContainsKey(assetName))
                {
                    assetObject = LoadAssets[assetName] as T;
                    return assetObject;
                }

                foreach(string dependAssetName in AssetManagerRuntime.Instance.Manifest.GetAllDependencies(info.AssetBundleName))
                {
                    string dependAssetBundlePath = Path.Combine(AssetManagerRuntime.Instance.AssetBundleLoadPath, dependAssetName);

                    AssetBundle.LoadFromFile(dependAssetBundlePath);
                }

                string assetBundlePath= Path.Combine(AssetManagerRuntime.Instance.AssetBundleLoadPath, info.AssetBundleName);

                AssetBundle bundle = AssetBundle.LoadFromFile(assetBundlePath);
                assetObject = bundle.LoadAsset<T>(assetName);
            }
        }

        if (assetObject == null)
        {
            Debug.LogError($"{assetName}未在{PackageName}中找到");
        }
        return assetObject;
    }
}
public class AssetManagerRuntime 
{
    /// <summary>
    /// 当前类的单例
    /// </summary>
    public static AssetManagerRuntime Instance;

    /// <summary>
    /// 当前资源包模式
    /// </summary>
    AssetBundlePattern CurrentPattern;

    /// <summary>
    /// 所有本地Asset所处的路径
    /// 应为AssetBundleLoadPath的上一层
    /// </summary>
    public string LocalAssetPath;

    /// <summary>
    /// AssetBundle加载路径
    /// </summary>
    public string AssetBundleLoadPath;

    /// <summary>
    /// 资源下载路径
    /// 下载完成后应该将资源放置到LocalAssetPath中
    /// </summary>
    public string DownloadPath;

    /// <summary>
    /// 用于对比本地资源版本和远端资源版本号
    /// </summary>
    public int LocalAssetVersion;

    /// <summary>
    /// 本次运行访问到的资源服务器版本号
    /// </summary>
    public int RemoteAssetVersion;

    /// <summary>
    /// 本地所有的Package信息
    /// </summary>
    List<string> PackageNames;

    /// <summary>
    /// 代表所有已加载的Package
    /// </summary>
    Dictionary<string, AssetPackage> LoadAssetPackages = new Dictionary<string, AssetPackage>();

    public AssetBundleManifest Manifest;

    public static void AssetManagerInit(AssetBundlePattern pattern)
    {
        if(Instance==null)
        {
            Instance = new AssetManagerRuntime();
            Instance.CurrentPattern = pattern;
            Instance.CheckLocalAssetPath();
            Instance.CheckLocalAssetVersion();
            Instance.CheckAssetBundleLoadPath();
        }
    }

    void CheckLocalAssetPath()
    {
        switch(CurrentPattern)
        {
            case AssetBundlePattern.EditorSimulation:
                break;
            case AssetBundlePattern.Local:
                LocalAssetPath = Path.Combine(Application.streamingAssetsPath, "LocalAssets");
                break;
            case AssetBundlePattern.Remote:
                DownloadPath = Path.Combine(Application.persistentDataPath, "DownloadAssets");
                if (!Directory.Exists(DownloadPath))
                {
                    Directory.CreateDirectory(DownloadPath);
                }
                LocalAssetPath = Path.Combine(Application.persistentDataPath, "LocalAssets");
                break;
        }
        if (!Directory.Exists(LocalAssetPath))
        {
            Directory.CreateDirectory(LocalAssetPath);
        }
    }

    void CheckLocalAssetVersion()
    {
        //asset.version是我们自定义拓展名的文本文件
        string versionFilePath = Path.Combine(LocalAssetPath, "LocalVersion.version");
        
        if(!File.Exists(versionFilePath))
        {
            LocalAssetVersion = 100;
            File.WriteAllText(versionFilePath, LocalAssetVersion.ToString());
            return;
        }
        LocalAssetVersion = int.Parse(File.ReadAllText(versionFilePath));
    }

    void CheckAssetBundleLoadPath()
    {
        AssetBundleLoadPath = Path.Combine(LocalAssetPath, LocalAssetVersion.ToString());
    }

    public void UpdateLocalAssetVersion()
    {
        LocalAssetVersion = RemoteAssetVersion;
        string versionFilePath = Path.Combine(LocalAssetPath, "LocalVersion.version");

        File.WriteAllText(versionFilePath, LocalAssetVersion.ToString());

        CheckLocalAssetPath();

        Debug.Log($"本地版本更新完成{LocalAssetVersion}");
    }

    public AssetPackage LoadPackage(string packageName)
    {
        string packagePath = null;
        string packageString = null;
        if (PackageNames==null)
        {
            packagePath = Path.Combine(AssetBundleLoadPath, "AllPackages");
            packageString = File.ReadAllText(packagePath);
            PackageNames = JsonConvert.DeserializeObject<List<string>>(packageString);
        }

        if(!PackageNames.Contains(packageName))
        {
            Debug.LogError($"{packageName}本地包列表中不存在该包");
            return null;
        }

        if(Manifest==null)
        {
            string mainBundlePath = Path.Combine(AssetBundleLoadPath, "LocalAssets");

            AssetBundle mainBundle = AssetBundle.LoadFromFile(mainBundlePath);

            Manifest = mainBundle.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
        }
        AssetPackage assetPackage = null;
        if(LoadAssetPackages.ContainsKey(packageName))
        {
            assetPackage = LoadAssetPackages[packageName];
            Debug.LogWarning($"{packageName}已经加载");
            return assetPackage;
        }
        assetPackage = new AssetPackage();
        packagePath = Path.Combine(AssetBundleLoadPath, packageName);
        packageString = File.ReadAllText(packagePath);
        assetPackage.PackageInfo = JsonConvert.DeserializeObject<PackageBuildInfo>(packageString);
        LoadAssetPackages.Add(assetPackage.PackageName, assetPackage);

        foreach(string dependName in assetPackage.PackageInfo.PackageDependencies)
        {
            LoadPackage(dependName);
        }
        return assetPackage;
    }

}
