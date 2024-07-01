using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum AssetBundlePattern
{
    /// <summary>
    /// �༭��ģ����أ�Ӧ��ʹ��AssetDataBase������Դ���أ������ý��д��
    /// </summary>
    EditorSimulation,
    /// <summary>
    /// ���ؼ���ģʽ��Ӧ���������·����StreamingAssets·���£��Ӹ�·������
    /// </summary>
    Local,
    /// <summary>
    /// Զ�˼���ģʽ��Ӧ�����������Դ��������ַ��Ȼ��ͨ�������������
    /// ���ص�ɳ��·��persistentDataPath���ٽ��м���
    /// </summary>
    Remote
}

/// <summary>
/// ��ΪAssets�µ������ű��ᱻ���뵽AssemblyCsharp.dll��
/// �����Ű�������ȥ����APK��,���Բ�����ʹ������UnityEditor�����ռ��µķ���
/// </summary>
public class HelloWorld : MonoBehaviour
{
    public AssetBundlePattern LoadPattern;

    AssetBundle SampleBundle;
    GameObject SampleObject;

    public Button LoadAssetBundleButton;
    public Button LoadAssetButton;
    public Button UnloadFalseButton;
    public Button UnloadTrueButton;

    /// <summary>
    /// ����İ�����Ӧ����Editor���������Ϊ��Դ����Ҳ��Ҫ����
    /// ���Է�����Դ��������
    /// </summary>
    public static string MainAssetBundleName = "SampleAssetBundle";

    /// <summary>
    /// ���������⣬ʵ�ʰ���������ȫ��Сд
    /// </summary>
    public static string ObjectAssetBundleName = "resourcesbundle";

    public string AssetBundleLoadPath;

    public string HTTPAddress = "http://10.24.4.179:8080/";

    public string HTTPAssetBundlePath;

    public string DownloadPath;
    void Start()
    {
        CheckAssetBundleLoadPath();
        LoadAssetBundleButton.onClick.AddListener(CheckAssetBundlePattern);
        LoadAssetButton.onClick.AddListener(LoadAsset);
        UnloadFalseButton.onClick.AddListener(() => { UnloadAssetBundle(false); });
        UnloadTrueButton.onClick.AddListener(() => { UnloadAssetBundle(true); });
    }

    void CheckAssetBundleLoadPath()
    {
        switch (LoadPattern)
        {
            case AssetBundlePattern.EditorSimulation:
                break;
            case AssetBundlePattern.Local:
                AssetBundleLoadPath = Path.Combine(Application.streamingAssetsPath, MainAssetBundleName);
                break;
            case AssetBundlePattern.Remote:
                HTTPAssetBundlePath = Path.Combine(HTTPAddress, MainAssetBundleName);
                DownloadPath = Path.Combine(Application.persistentDataPath, "DownloadAssetBundle");
                AssetBundleLoadPath = Path.Combine(DownloadPath, MainAssetBundleName);
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
            //�������ֽ���
            Debug.Log(webRequest.downloadedBytes);
            //���ؽ���
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
            //��Ŀ������ж϶����Ƿ�Ϊ��
            callBack?.Invoke();        
        }
    }

    IEnumerator SaveFile(string savePath,byte[] bytes,Action callBack)
    {
        //���е�System.IO������ֻ����Windowsƽ̨������
        //�����Ҫ��ƽ̨�����ļ���Ӧ��ÿ��ƽ̨���ò�ͬ��API
        FileStream fileStream = File.Open(savePath,FileMode.OpenOrCreate);

        yield return fileStream.WriteAsync(bytes,0,bytes.Length);
        //ˢ���ļ�״̬
        fileStream.Flush();
        //�ر�
        fileStream.Close();
        //�ͷ��ļ����������ļ���һֱ���ڶ�ȡ״̬�����ܱ��������̶�ȡ
        fileStream.Dispose();

        callBack?.Invoke();
        Debug.Log($"{savePath}�ļ��������");
    }

    void CheckAssetBundlePattern()
    {
        if (LoadPattern == AssetBundlePattern.Remote)
        {
            StartCoroutine(DownloadFile(ObjectAssetBundleName, LoadAssetBundle));
        }
    }
    void LoadAssetBundle()
    {
        //ͨ���ⲿ·������AB���ķ�ʽ
        //��ΪpersistentDataPath���ƶ��˿ɶ���д������
        //Զ�����ص�AB�������Է����ڸ�·����
        //AB�����ؿ���������ع���·�����·�������ط�ʽ��Unityά��

        string assetBundlePath = Path.Combine(AssetBundleLoadPath,MainAssetBundleName);
        //�����嵥�����
        AssetBundle mainAB = AssetBundle.LoadFromFile(assetBundlePath);

        //manifest�ļ�ʵ���������Ĵ�������ǿ����߲���������
        AssetBundleManifest assetBundleManifest = mainAB.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));

        //manifest.GetAllDependencies��ȡ����һ��AB������ֱ�ӻ��ӵ�����
        //Ϊ����ĳЩ������õ���Դû�б����ص�������ʹ��GetAllDependencies
        //manifest.GetDirectDependencies��ȡ����ֱ�ӵ�����
        foreach (string depAssetBundleName in assetBundleManifest.GetAllDependencies(ObjectAssetBundleName))
        {
            Debug.Log(depAssetBundleName);
            assetBundlePath = Path.Combine(AssetBundleLoadPath, depAssetBundleName);
            //�������Ҫʹ��������ʵ�������Բ��ñ��������ʵ�������Ǹ�ʵ����Ȼ��������ڴ���
            AssetBundle.LoadFromFile(assetBundlePath);
        }

        assetBundlePath = Path.Combine(AssetBundleLoadPath, ObjectAssetBundleName);

        SampleBundle = AssetBundle.LoadFromFile(assetBundlePath);
    }

    void LoadAsset()
    {
        GameObject cubeObject = SampleBundle.LoadAsset<GameObject>("Cube");
        SampleObject = Instantiate(cubeObject);
    }

    void UnloadAssetBundle(bool isTrue)
    {
        Debug.Log(isTrue);
        //��ǰ֡���ٶ���
        DestroyImmediate(SampleObject);
        SampleBundle.Unload(isTrue);

        //ʹ��unload(false)������һ�������������ƣ����ǲ����ƻ���ǰ����ʱ��Ч��
        //�����ʲô��Դ��AB������������û�б�����������Դ��Ȼ��ʹ�ö�AB��ʹ��unload(true)����ж��
        //�ͻᵼ�µ�ǰ����ʱͻȻ��ʧĳЩ����ж��AB������Դ
        //��ô����Ȼ�ģ��ڲ��ƻ�����ʱ��Ч��������£�Ҳ���ǵ���unload(false)������£���ʹ��Resources.unloadUnusedAsset()����������
        //��Ч����õ�
        //��Ϊ���ж��ڴ�Ĳ���������ռ��CPU��ʹ�ã����������CPUʹ������ϵ͵�����½���ǿ�Ƶ���Դж��
        //������Ϸ�����������򳡾�����ʱ
        Resources.UnloadUnusedAssets();
    }
}
