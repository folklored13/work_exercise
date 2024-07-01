using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class PlayerData
{
    public static int MoveSpeed;
    public static int PotateSpeed;
}


/// <summary>
/// 因为Assets下的其他脚本会被编译到AssemblyCsharp.dll中
/// 跟随着包体打包出去（如APK）,所以不允许使用来自UnityEditor命名空间下的方法
/// </summary>
public class HelloWorld : MonoBehaviour
{
    public AssetBundlePattern LoadPattern;

    AssetBundle CubeBundle;
    AssetBundle SphereBundle;
    GameObject SampleObject;

    public Button LoadAssetBundleButton;
    public Button LoadAssetButton;
    public Button UnloadFalseButton;
    public Button UnloadTrueButton;

    /// <summary>
    /// 打包的包名本应该由Editor类管理，但因为资源加载也需要访问
    /// 所以放在资源加载类中
    /// </summary>
    //public static string MainAssetBundleName = "SampleAssetBundle";

    /// <summary>
    /// 除了主包外，实际包名都必须全部小写
    /// </summary>
    //public static string ObjectAssetBundleName = "resourcesbundle";

    public string AssetBundleLoadPath;

    public string HTTPAddress = "http://192.168.203.59:8080/";

    public string HTTPAssetBundlePath;

    public string DownloadPath;
    void Start()
    {
        AssetManagerRuntime.AssetManagerInit(LoadPattern);
        if (LoadPattern == AssetBundlePattern.Remote)
        {
            StartCoroutine(GetRemoteVersion());
        }
        else
        {
            LoadAsset();
        }


        //CheckAssetBundleLoadPath();
        //LoadAssetBundleButton.onClick.AddListener(CheckAssetBundlePattern);
        //LoadAssetButton.onClick.AddListener(LoadAsset);
        //UnloadFalseButton.onClick.AddListener(() => { UnloadAssetBundle(false); });
        //UnloadTrueButton.onClick.AddListener(() => { UnloadAssetBundle(true); });
    }
    void LoadAsset()
    {
        AssetPackage assetPackage = AssetManagerRuntime.Instance.LoadPackage("AA");

        Debug.Log(assetPackage.PackageName);

        GameObject sampleObject = assetPackage.LoadAsset<GameObject>("Assets/Resources/Capsule.prefab");
        Instantiate(sampleObject);
    }
    IEnumerator GetRemoteVersion()
    {
        string remoteVersionFilePath = Path.Combine(HTTPAddress, "BuildOutput", "BuildVersion.version");

        UnityWebRequest request = UnityWebRequest.Get(remoteVersionFilePath);

        request.SendWebRequest();

        while(!request.isDone)
        {
            //返回null代表等待一帧
            yield return null;
        }

        if(!string.IsNullOrEmpty(request.error))
        {
            Debug.LogError(request.error);
            yield break;
        }

        int version = int.Parse(request.downloadHandler.text);

        AssetManagerRuntime.Instance.RemoteAssetVersion = version;
        
        Debug.Log($"远端资源版本为{version}");

        string downloadPath = Path.Combine(AssetManagerRuntime.Instance.DownloadPath,
            AssetManagerRuntime.Instance.RemoteAssetVersion.ToString());

        if (!Directory.Exists(downloadPath))
        {
            Directory.CreateDirectory(downloadPath);
        }

        if(AssetManagerRuntime.Instance.LocalAssetVersion != AssetManagerRuntime.Instance.RemoteAssetVersion)
        {
            StartCoroutine(GetRemotePackages());
        }
        else
        {
            LoadAsset();
        }
        
        yield return null;
    }

    IEnumerator GetRemotePackages()
    {
        string remotePackagePath = Path.Combine(HTTPAddress, "BuildOutput", AssetManagerRuntime.Instance.RemoteAssetVersion.ToString(), "AllPackages");

        UnityWebRequest request = UnityWebRequest.Get(remotePackagePath);

        request.SendWebRequest();

        while (!request.isDone)
        {
            //返回null代表等待一帧
            yield return null;
        }

        if (!string.IsNullOrEmpty(request.error))
        {
            Debug.LogError(request.error);
            yield break;
        }

        string allPackagesString = request.downloadHandler.text;
        
        string packagesSavePath = Path.Combine(AssetManagerRuntime.Instance.DownloadPath,
            AssetManagerRuntime.Instance.RemoteAssetVersion.ToString(), "AllPackages");

        File.WriteAllText(packagesSavePath, allPackagesString);

        Debug.Log($"Packages下载完毕{packagesSavePath}");

        //-----------把每一个Package的具体文件下载下来------------

        //将之前下载好的allPackage表格转成ListString，并下载对应包到本地
        List<string> packagesNames = JsonConvert.DeserializeObject<List<string>>(allPackagesString);

        foreach(string packageName in packagesNames)
        {
            remotePackagePath = Path.Combine(HTTPAddress, "BuildOutput", AssetManagerRuntime.Instance.RemoteAssetVersion.ToString(), packageName);

            request = UnityWebRequest.Get(remotePackagePath);

            request.SendWebRequest();

            while (!request.isDone)
            {
                //返回null代表等待一帧
                yield return null;
            }

            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                yield break;
            }

            string packageString = request.downloadHandler.text;

            packagesSavePath = Path.Combine(AssetManagerRuntime.Instance.DownloadPath,
            AssetManagerRuntime.Instance.RemoteAssetVersion.ToString(), packageName);

            File.WriteAllText(packagesSavePath, packageString);

            Debug.Log($"package下载完毕{packageName}");
        }

        StartCoroutine(GetRemoteAssetBundleHash());
        yield return null;
    }

    IEnumerator GetRemoteAssetBundleHash()
    {
        string remoteHashPath = Path.Combine(HTTPAddress, "BuildOutput", AssetManagerRuntime.Instance.RemoteAssetVersion.ToString(), "AssetBundleHashs");

        UnityWebRequest request = UnityWebRequest.Get(remoteHashPath);

        request.SendWebRequest();

        while (!request.isDone)
        {
            //返回null代表等待一帧
            yield return null;
        }

        if (!string.IsNullOrEmpty(request.error))
        {
            Debug.LogError(request.error);
            yield break;
        }

        string hashString = request.downloadHandler.text;
        string hashSavePath = Path.Combine(AssetManagerRuntime.Instance.DownloadPath,
            AssetManagerRuntime.Instance.RemoteAssetVersion.ToString(), "AssetBundleHashs");

        File.WriteAllText(hashSavePath, hashString);

        Debug.Log($"AssetBundleHash列表下载完成{hashString}");

        CreateDownloadList();
        yield return null;
    }

    void CreateDownloadList()
    {
        //首先分别读取本地AssetBundleHash列表和远端AssetBundleHash列表
        string localAssetBundleHashPath = Path.Combine(AssetManagerRuntime.Instance.AssetBundleLoadPath, "AssetBundleHashs");
        string assetBundleHashString = "";
        string[] localAssetBundleHash = null;

        if(File.Exists(localAssetBundleHashPath))
        {
            assetBundleHashString = File.ReadAllText(localAssetBundleHashPath);

            localAssetBundleHash = JsonConvert.DeserializeObject<string[]>(assetBundleHashString);
        }

        string remoteAssetBundleHashPath = Path.Combine(AssetManagerRuntime.Instance.DownloadPath,
            AssetManagerRuntime.Instance.RemoteAssetVersion.ToString(), "AssetBundleHashs");
        string remoteAssetBundleHashString = "";
        string[] remoteAssetBundleHash = null;

        if (File.Exists(remoteAssetBundleHashPath))
        {
            remoteAssetBundleHashString = File.ReadAllText(remoteAssetBundleHashPath);
            remoteAssetBundleHash = JsonConvert.DeserializeObject<string[]>(remoteAssetBundleHashString);
        }

        //读取完成后进行判断，如果远端表不存在则直接返回，无法更新，如果本地表不存在，则下载列表等于远端表
        //如果两者都存在，则对比版本差异，将新增AssetBundle内容作为下载列表
        if(remoteAssetBundleHash==null)
        {
            Debug.LogError($"远端表读取失败{remoteAssetBundleHashPath}");
            return;
        }

        //将要下载的AB包名称
        List<string> assetBundleNames = null;

        if(localAssetBundleHash==null)
        {
            Debug.LogWarning("本地表读取失败");
            assetBundleNames = remoteAssetBundleHash.ToList();
            
        }
        else
        {
            AssetBundleVersionDifference versionDifference = ContrastAssetBundleVersion(localAssetBundleHash, remoteAssetBundleHash);
            
            //新增AB包列表就是将要下载的文件列表
            assetBundleNames = versionDifference.AdditionAssetBundles;
        }

        if(assetBundleNames != null && assetBundleNames.Count>0)
        {
            //添加主包包名
            assetBundleNames.Add("LocalAssets");

            StartCoroutine(DownloadAssetBundle(assetBundleNames, () => { 
                CopyDownloadAssetsToLocalPath();
                AssetManagerRuntime.Instance.UpdateLocalAssetVersion();
                LoadAsset();
            }));
        }
    }

    void CopyDownloadAssetsToLocalPath()
    {
        string downloadAssetVersionPath = Path.Combine(AssetManagerRuntime.Instance.DownloadPath,
            AssetManagerRuntime.Instance.RemoteAssetVersion.ToString());

        DirectoryInfo directoryInfo = new DirectoryInfo(downloadAssetVersionPath);
        string localVersionPath = Path.Combine(AssetManagerRuntime.Instance.LocalAssetPath, AssetManagerRuntime.Instance.RemoteAssetVersion.ToString());
        directoryInfo.MoveTo(localVersionPath);
    }

    IEnumerator DownloadAssetBundle(List<string> fileNames,Action callBack=null)
    {
        foreach(string fileName in fileNames)
        {
            string assetBundleName = fileName;
            if(fileName.Contains("_"))
            {
                //下划线后一位才是AssetBundleName
                int startIndex = fileName.IndexOf("_") + 1;
                assetBundleName = fileName.Substring(startIndex);
            }

            string assetBundleDownloadPath = Path.Combine(HTTPAddress, "BuildOutput",
                AssetManagerRuntime.Instance.RemoteAssetVersion.ToString(), assetBundleName);

            UnityWebRequest request = UnityWebRequest.Get(assetBundleDownloadPath);

            request.SendWebRequest();

            while (!request.isDone)
            {
                //返回null代表等待一帧
                yield return null;
            }

            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                yield break;
            }

            string assetBundleSavePath = Path.Combine(AssetManagerRuntime.Instance.DownloadPath,
            AssetManagerRuntime.Instance.RemoteAssetVersion.ToString(), assetBundleName);

            File.WriteAllBytes(assetBundleSavePath, request.downloadHandler.data);

            Debug.Log($"AssetBundle下载完毕{assetBundleName}");
        }

        callBack?.Invoke();
        //if(callBack != null)
        //{
        //    callBack.Invoke();
        //}

        yield return null;
    }

    static AssetBundleVersionDifference ContrastAssetBundleVersion(string[] oldVersionAssets, string[] newVersionAssets)
    {
        AssetBundleVersionDifference difference = new AssetBundleVersionDifference();
        foreach (var assetName in oldVersionAssets)
        {
            if (!newVersionAssets.Contains(assetName))
            {
                difference.ReducedAssetBundles.Add(assetName);
            }
        }

        foreach (var assetName in newVersionAssets)
        {
            if (!oldVersionAssets.Contains(assetName))
            {
                difference.AdditionAssetBundles.Add(assetName);
            }
        }

        return difference;
    }

    void CheckAssetBundleLoadPath()
    {
        switch (LoadPattern)
        {
            case AssetBundlePattern.EditorSimulation:
                break;
            case AssetBundlePattern.Local:
                AssetBundleLoadPath = Path.Combine(Application.streamingAssetsPath);
                break;
            case AssetBundlePattern.Remote:
                HTTPAssetBundlePath = Path.Combine(HTTPAddress);
                DownloadPath = Path.Combine(Application.persistentDataPath, "DownloadAssetBundle");
                AssetBundleLoadPath = Path.Combine(DownloadPath);
                if (!Directory.Exists(AssetBundleLoadPath))
                {
                    Directory.CreateDirectory(AssetBundleLoadPath);
                }
                break;
        }
    }


    IEnumerator DownloadFile(string fileName, Action callBack, bool isSaveFile=true)
    {
        string assetBundleDownloadPath = Path.Combine(HTTPAssetBundlePath, fileName);

        UnityWebRequest webRequest = UnityWebRequest.Get(assetBundleDownloadPath);

        yield return webRequest.SendWebRequest();

        while(!webRequest.isDone)
        {
            //下载总字节数
            Debug.Log(webRequest.downloadedBytes);
            //下载进度
            Debug.Log(webRequest.downloadProgress);
            yield return new WaitForEndOfFrame();
        }
        string fileSavePath = Path.Combine(AssetBundleLoadPath, fileName);
        Debug.Log(webRequest.downloadHandler.data.Length);
        if(isSaveFile)
        {
            yield return SaveFile(fileSavePath, webRequest.downloadHandler.data,callBack);
        }
        else
        {
            //三目运算符判断对象是否为空
            callBack?.Invoke();        
        }
    }

    IEnumerator SaveFile(string savePath,byte[] bytes,Action callBack)
    {
        //所有的System.IO方法都只能在Windows平台上运行
        //如果想要跨平台保存文件，应该每个平台调用不同的API
        FileStream fileStream = File.Open(savePath,FileMode.OpenOrCreate);

        yield return fileStream.WriteAsync(bytes,0,bytes.Length);
        //刷新文件状态
        fileStream.Flush();
        //关闭
        fileStream.Close();
        //释放文件流，否则文件会一直处于读取状态而不能被其他进程读取
        fileStream.Dispose();

        callBack?.Invoke();
        Debug.Log($"{savePath}文件保存完成");
    }

 }
