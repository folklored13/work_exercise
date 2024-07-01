using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

<<<<<<< HEAD
=======
public static class PlayerData
{
    public static int MoveSpeed;
    public static int PotateSpeed;
}

>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
public enum AssetBundlePattern
{
    /// <summary>
    /// ±à¼­Æ÷Ä£Äâ¼ÓÔØ£¬Ó¦¸ÃÊ¹ÓÃAssetDataBase½øĞĞ×ÊÔ´¼ÓÔØ£¬¶ø²»ÓÃ½øĞĞ´ò°ü
    /// </summary>
    EditorSimulation,
    /// <summary>
    /// ±¾µØ¼ÓÔØÄ£Ê½£¬Ó¦´ò°üµ½±¾µØÂ·¾¶»òStreamingAssetsÂ·¾¶ÏÂ£¬´Ó¸ÃÂ·¾¶¼ÓÔØ
    /// </summary>
    Local,
    /// <summary>
    /// Ô¶¶Ë¼ÓÔØÄ£Ê½£¬Ó¦´ò°üµ½ÈÎÒâ×ÊÔ´·şÎñÆ÷µØÖ·£¬È»ºóÍ¨¹ıÍøÂç½øĞĞÏÂÔØ
    /// ÏÂÔØµ½É³ºĞÂ·¾¶persistentDataPathºó£¬ÔÙ½øĞĞ¼ÓÔØ
    /// </summary>
    Remote
}

/// <summary>
/// ÒòÎªAssetsÏÂµÄÆäËû½Å±¾»á±»±àÒëµ½AssemblyCsharp.dllÖĞ
/// ¸úËæ×Å°üÌå´ò°ü³öÈ¥£¨ÈçAPK£©,ËùÒÔ²»ÔÊĞíÊ¹ÓÃÀ´×ÔUnityEditorÃüÃû¿Õ¼äÏÂµÄ·½·¨
/// </summary>
public class HelloWorld : MonoBehaviour
{
    public AssetBundlePattern LoadPattern;

<<<<<<< HEAD
    AssetBundle SampleBundle;
=======
    AssetBundle CubeBundle;
    AssetBundle SphereBundle;
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
    GameObject SampleObject;

    public Button LoadAssetBundleButton;
    public Button LoadAssetButton;
    public Button UnloadFalseButton;
    public Button UnloadTrueButton;

    /// <summary>
    /// ´ò°üµÄ°üÃû±¾Ó¦¸ÃÓÉEditorÀà¹ÜÀí£¬µ«ÒòÎª×ÊÔ´¼ÓÔØÒ²ĞèÒª·ÃÎÊ
    /// ËùÒÔ·ÅÔÚ×ÊÔ´¼ÓÔØÀàÖĞ
    /// </summary>
    public static string MainAssetBundleName = "SampleAssetBundle";

    /// <summary>
    /// ³ıÁËÖ÷°üÍâ£¬Êµ¼Ê°üÃû¶¼±ØĞëÈ«²¿Ğ¡Ğ´
    /// </summary>
    public static string ObjectAssetBundleName = "resourcesbundle";

    public string AssetBundleLoadPath;

    public string HTTPAddress = "http://10.24.4.179:8080/";

    public string HTTPAssetBundlePath;

    public string DownloadPath;
    void Start()
    {
<<<<<<< HEAD
=======
        
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
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
            //ÏÂÔØ×Ü×Ö½ÚÊı
            Debug.Log(webRequest.downloadedBytes);
            //ÏÂÔØ½ø¶È
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
            //ÈıÄ¿ÔËËã·ûÅĞ¶Ï¶ÔÏóÊÇ·ñÎª¿Õ
            callBack?.Invoke();        
        }
    }

    IEnumerator SaveFile(string savePath,byte[] bytes,Action callBack)
    {
        //ËùÓĞµÄSystem.IO·½·¨¶¼Ö»ÄÜÔÚWindowsÆ½Ì¨ÉÏÔËĞĞ
        //Èç¹ûÏëÒª¿çÆ½Ì¨±£´æÎÄ¼ş£¬Ó¦¸ÃÃ¿¸öÆ½Ì¨µ÷ÓÃ²»Í¬µÄAPI
        FileStream fileStream = File.Open(savePath,FileMode.OpenOrCreate);

        yield return fileStream.WriteAsync(bytes,0,bytes.Length);
        //Ë¢ĞÂÎÄ¼ş×´Ì¬
        fileStream.Flush();
        //¹Ø±Õ
        fileStream.Close();
        //ÊÍ·ÅÎÄ¼şÁ÷£¬·ñÔòÎÄ¼ş»áÒ»Ö±´¦ÓÚ¶ÁÈ¡×´Ì¬¶ø²»ÄÜ±»ÆäËû½ø³Ì¶ÁÈ¡
        fileStream.Dispose();

        callBack?.Invoke();
        Debug.Log($"{savePath}ÎÄ¼ş±£´æÍê³É");
    }

    void CheckAssetBundlePattern()
    {
<<<<<<< HEAD
=======
        
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
        if (LoadPattern == AssetBundlePattern.Remote)
        {
            StartCoroutine(DownloadFile(ObjectAssetBundleName, LoadAssetBundle));
        }
<<<<<<< HEAD
=======
        else
        {
            LoadAssetBundle();
        }
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
    }
    void LoadAssetBundle()
    {
        //Í¨¹ıÍâ²¿Â·¾¶¼ÓÔØAB°üµÄ·½Ê½
        //ÒòÎªpersistentDataPathÔÚÒÆ¶¯¶Ë¿É¶Á¿ÉĞ´µÄÌØĞÔ
        //Ô¶³ÌÏÂÔØµÄAB°ü¶¼¿ÉÒÔ·ÅÖÃÔÚ¸ÃÂ·¾¶ÏÂ
        //AB°ü¼ÓÔØ¿ÉÒÔÔÊĞí¼ÓÔØ¹¤³ÌÂ·¾¶ÍâµÄÂ·¾¶£¬¼ÓÔØ·½Ê½ÓÉUnityÎ¬»¤

        string assetBundlePath = Path.Combine(AssetBundleLoadPath,MainAssetBundleName);
        //¼ÓÔØÇåµ¥À¦°ó°ü
        AssetBundle mainAB = AssetBundle.LoadFromFile(assetBundlePath);

        //manifestÎÄ¼şÊµ¼ÊÉÏÊÇÃ÷ÎÄ´¢´æ¸øÎÒÃÇ¿ª·¢Õß²éÕÒË÷ÒıµÄ
        AssetBundleManifest assetBundleManifest = mainAB.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));

        //manifest.GetAllDependencies»ñÈ¡µÄÊÇÒ»¸öAB°üËùÓĞÖ±½Ó»ò¼ä½ÓµÄÒıÓÃ
        //Îª±ÜÃâÄ³Ğ©¼ä½ÓÒıÓÃµÄ×ÊÔ´Ã»ÓĞ±»¼ÓÔØµ½£¬½¨ÒéÊ¹ÓÃGetAllDependencies
        //manifest.GetDirectDependencies»ñÈ¡µÄÊÇÖ±½ÓµÄÒıÓÃ
<<<<<<< HEAD
        foreach (string depAssetBundleName in assetBundleManifest.GetAllDependencies(ObjectAssetBundleName))
=======
        foreach (string depAssetBundleName in assetBundleManifest.GetAllDependencies("1"))
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
        {
            Debug.Log(depAssetBundleName);
            assetBundlePath = Path.Combine(AssetBundleLoadPath, depAssetBundleName);
            //Èç¹û²»ĞèÒªÊ¹ÓÃÒÀÀµ°üÊµÀı£¬¿ÉÒÔ²»ÓÃ±äÁ¿´¢´æ¸ÃÊµÀı£¬µ«ÊÇ¸ÃÊµÀıÈÔÈ»»á´æÔÚÓÚÄÚ´æÖĞ
            AssetBundle.LoadFromFile(assetBundlePath);
        }

<<<<<<< HEAD
        assetBundlePath = Path.Combine(AssetBundleLoadPath, ObjectAssetBundleName);

        SampleBundle = AssetBundle.LoadFromFile(assetBundlePath);
=======
        assetBundlePath = Path.Combine(AssetBundleLoadPath, "1");

        CubeBundle = AssetBundle.LoadFromFile(assetBundlePath);

        assetBundlePath = Path.Combine(AssetBundleLoadPath, "2");

        SphereBundle = AssetBundle.LoadFromFile(assetBundlePath);
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
    }

    void LoadAsset()
    {
<<<<<<< HEAD
        GameObject cubeObject = SampleBundle.LoadAsset<GameObject>("Cube");
        SampleObject = Instantiate(cubeObject);
=======
        GameObject cubeObject = CubeBundle.LoadAsset<GameObject>("Cube");
        Instantiate(cubeObject);
        cubeObject = SphereBundle.LoadAsset<GameObject>("Sphere");
        Instantiate(cubeObject);
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
    }

    void UnloadAssetBundle(bool isTrue)
    {
        Debug.Log(isTrue);
        //µ±Ç°Ö¡Ïú»Ù¶ÔÏó
        DestroyImmediate(SampleObject);
<<<<<<< HEAD
        SampleBundle.Unload(isTrue);
=======
        //CubeBundle.Unload(isTrue);
        //SphereBundle.Unload(isTrue);
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)

        //Ê¹ÓÃunload(false)·½·¨ÓĞÒ»¸öºÜÏÔÖøµÄÓÅÊÆ£¬¾ÍÊÇ²»»áÆÆ»µµ±Ç°ÔËĞĞÊ±µÄĞ§¹û
        //Èç¹ûÓĞÊ²Ã´×ÊÔ´ÊÇAB°ü´´½¨£¬µ«ÊÇÃ»ÓĞ±»¹ÜÀí£¬µ¼ÖÂ×ÊÔ´ÈÔÈ»±»Ê¹ÓÃ¶øAB°üÊ¹ÓÃunload(true)·½·¨Ğ¶ÔØ
        //¾Í»áµ¼ÖÂµ±Ç°ÔËĞĞÊ±Í»È»¶ªÊ§Ä³Ğ©À´×ÔĞ¶ÔØAB°üµÄ×ÊÔ´
        //ÄÇÃ´ºÜÏÔÈ»µÄ£¬ÔÚ²»ÆÆ»µÔËĞĞÊ±µÄĞ§¹ûµÄÇé¿öÏÂ£¨Ò²¾ÍÊÇµ÷ÓÃunload(false)µÄÇé¿öÏÂ£©£¬Ê¹ÓÃResources.unloadUnusedAsset()·½·¨À´»ØÊÕ
        //ÊÇĞ§¹û×îºÃµÄ
        //ÒòÎªËùÓĞ¶ÔÄÚ´æµÄ²Ù×÷£¬¶¼»áÕ¼¾İCPUµÄÊ¹ÓÃ£¬ËùÒÔ×îºÃÔÚCPUÊ¹ÓÃÇé¿ö½ÏµÍµÄÇé¿öÏÂ½øĞĞÇ¿ÖÆµÄ×ÊÔ´Ğ¶ÔØ
        //ÀıÈçÓÎÏ·¹ı³¡¶¯»­£¬»ò³¡¾°¼ÓÔØÊ±
        Resources.UnloadUnusedAssets();
    }
}
