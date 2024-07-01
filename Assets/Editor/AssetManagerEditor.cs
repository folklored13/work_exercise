using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
<<<<<<< HEAD
=======
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)

public enum AssetBundleCompressionPattern
{
    LZMA,
    LZ4,
    None
}

/// <summary>
<<<<<<< HEAD
/// ËùÓĞÔÚEditorÄ¿Â¼ÏÂµÄC#½Å±¾¶¼²»»á¸ú×Å×ÊÔ´´ò°üµ½¿ÉÖ´ĞĞÎÄ¼ş°üÌåÖĞ
/// </summary>
public class AssetManagerEditor 
{
    //ÉùÃ÷°æ±¾ºÅ
    public static string AssetManagerVersion = "1.0.0";

    /// <summary>
    /// ±à¼­Æ÷Ä£ÄâÏÂ£¬²»½øĞĞ´ò°ü
    /// ±¾µØÄ£Ê½£¬´ò°üµ½StreamingAssets
    /// Ô¶¶ËÄ£Ê½£¬´ò°üµ½ÈÎÒâÔ¶¶ËÂ·¾¶£¬ÔÚ¸ÃÊ¾ÀıÖĞÎªpersistentDataPath
    /// </summary>
    public static AssetBundlePattern BuildingPattern;

    public static AssetBundleCompressionPattern CompressionPattern;

    /// <summary>
    /// ĞèÒª´ò°üµÄÎÄ¼ş¼Ğ
    /// </summary>
    public static DefaultAsset AssetBundleDirectory;



    public static string AssetBundleOutputPath;

    /// <summary>
    /// Í¨¹ıMenuItemÌØĞÔ£¬ÉùÃ÷Editor¶¥²¿²Ëµ¥À¸Ñ¡Ïî
    /// </summary>
    [MenuItem(nameof(AssetManagerEditor)+"/"+nameof(BuildAssetBundle))]
=======
/// ÈÎºÎBuildOption´¦ÓÚ·ÇforceRebuildÑ¡ÏîÏÂ¶¼Ä¬ÈÏÎªÔöÁ¿´ò°ü
/// </summary>
public enum IncrementalBuildMode
{
    None,
    IncrementalBuild,
    ForceRebuild
}

public class AssetBundleVersionDifference
{
    /// <summary>
    /// ĞÂÔö×ÊÔ´°ü
    /// </summary>
    public List<string> AdditionAssetBundles=new List<string>();
    /// <summary>
    /// ¼õÉÙ×ÊÔ´°ü
    /// </summary>
    public List<string> ReducedAssetBundles=new List<string>();
}

/// <summary>
///´ú±íÁËNodeÖ®¼äµÄÒıÓÃ¹ØÏµ£¬ºÜÏÔÈ»Ò»¸öNodeÖ®¼ä¿ÉÄÜÒıÓÃ¶à¸öNode£¬Ò²¿ÉÄÜ±»¶à¸öNodeËùÒıÓÃ
/// </summary>
public class AssetBundleEdge
{
    public List<AssetBundleNode> nodes = new List<AssetBundleNode>();
}

public class AssetBundleNode
{
    public string AssetName;
    /// <summary>
    /// ¿ÉÒÔÓÃÓëÅĞ¶ÏÒ»¸ö×ÊÔ´ÊÇ·ñÊÇSourceAsset£¬Èç¹ûÊÇ-1ËµÃ÷ÊÇDerivedAsset
    /// </summary>
    public int SourceIndex = -1;

    /// <summary>
    /// µ±Ç°NodeµÄIndexÁĞ±í£¬»áÑØ×Å×ÔÉíµÄOutEdge½øĞĞ´«µİ
    /// </summary>
    public List<int> SourceIndeices = new List<int>();
    /// <summary>
    /// µ±Ç°NodeËùÒıÓÃµÄNodes
    /// </summary>
    public AssetBundleEdge OutEdge;
    /// <summary>
    /// ÒıÓÃµ±Ç°NodeµÄNodes
    /// </summary>
    public AssetBundleEdge InEdge;
}


/// <summary>
/// ËùÓĞÔÚEditorÄ¿Â¼ÏÂµÄC#½Å±¾¶¼²»»á¸ú×Å×ÊÔ´´ò°üµ½¿ÉÖ´ĞĞÎÄ¼ş°üÌåÖĞ
/// </summary>
public class AssetManagerEditor
{
    public static AssetManagerConfigScriptableObject AssetManagerConfig;

    

    public static string AssetBundleOutputPath;


    /// <summary>
    /// Í¨¹ıMenuItemÌØĞÔ£¬ÉùÃ÷Editor¶¥²¿²Ëµ¥À¸Ñ¡Ïî
    /// </summary>
    [MenuItem(nameof(AssetManagerEditor) + "/" + nameof(BuildAssetBundle))]
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
    static void BuildAssetBundle()
    {
        CheckBuildOutputPath();
        //PathCombine·½·¨¿ÉÒÔÔÚ¼¸¸ö×Ö·û´®Ö®¼ä²åÈëĞ±¸Ü
        //string outputPath = "E:/AssetBundles/testAB1";
        //string outputPath = "E:/AssetBundles/testAB2";
        //string outputPath = "E:/AssetBundles/testAB3";

        if (!Directory.Exists(AssetBundleOutputPath))
        {
            Directory.CreateDirectory(AssetBundleOutputPath);
        }

        //²»Í¬Æ½Ì¨Ö®¼äµÄAssetBundle²»¿ÉÒÔÍ¨ÓÃ
        //¸Ã·½·¨»á´ò°ü¹¤³ÌÄÚËùÓĞÅäÖÃÁË°üÃûµÄAB°ü£¬¼´Èç¹ûÃ»ÓĞÉèÖÃ°üÃû¾Í´ò²»³ö°ü
        //OptionsÎªNoneÊ±Ê¹ÓÃLZMAÑ¹Ëõ
        //UncompressedAssetBundle²»½øĞĞÑ¹Ëõ
        //ChunkBasedCompression½øĞĞLZ4¿éÑ¹Ëõ

        BuildPipeline.BuildAssetBundles(AssetBundleOutputPath, CheckCompressionPattern(), EditorUserBuildSettings.activeBuildTarget);

        Debug.Log("AB°ü´ò°üÒÑÍê³É");
    }

<<<<<<< HEAD
    static BuildAssetBundleOptions CheckCompressionPattern()
    {
        BuildAssetBundleOptions option = new BuildAssetBundleOptions();
        switch (CompressionPattern)
=======
    
    [MenuItem(nameof(AssetManagerEditor) + "/" + nameof(CreateConfig))]
    static void CreateConfig()
    {
        //ÉùÃ÷ScriptableObjectÀàĞÍµÄÊµÀı
        //ScriptableObjectÀàĞÍµÄÉùÃ÷£¬ÀàËÆÓÚJSONÖĞ½«Ä³¸öÀàµÄÊµÀıĞòÁĞ»¯µÄ¹ı³Ì
        AssetManagerConfigScriptableObject config = ScriptableObject.CreateInstance<AssetManagerConfigScriptableObject>();

        AssetDatabase.CreateAsset(config, "Assets/Editor/AssetManagerConfig.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void LoadConfig(AssetManagerEditorWindow window)
    {
        if (AssetManagerConfig == null)
        {
            //Ê¹ÓÃAssetDatabase¼ÓÔØ×ÊÔ´£¬Ö»ĞèÒª´«ÈëAssetsÄ¿Â¼ÏÂµÄÂ·¾¶¼´¿É
            AssetManagerConfig = AssetDatabase.LoadAssetAtPath<AssetManagerConfigScriptableObject>("Assets/Editor/AssetManagerConfig.asset");
            window.VersionString = AssetManagerConfig.AssetManagerVersion.ToString();

            for(int i=window.VersionString.Length;i>=1;i--)
            {
                window.VersionString = window.VersionString.Insert(i, ".");
            }

            window.editorWindowDirectory = AssetManagerConfig.AssetBundleDirectory;
        }
    }

    public static void LoadWindowConfig(AssetManagerEditorWindow window)
    {
        if (window.WindowConfig == null)
        {
            //Ê¹ÓÃAssetDatabase¼ÓÔØ×ÊÔ´£¬Ö»ĞèÒª´«ÈëAssetsÄ¿Â¼ÏÂµÄÂ·¾¶¼´¿É
            window.WindowConfig = AssetDatabase.LoadAssetAtPath<AssetManagerEditorWindowConfigSO>("Assets/Editor/AssetManagerEditorWindowConfig.asset");
            window.WindowConfig.TitleTextStyle = new GUIStyle();
            window.WindowConfig.TitleTextStyle.fontSize = 26;
            window.WindowConfig.TitleTextStyle.normal.textColor = Color.red;
            window.WindowConfig.TitleTextStyle.alignment = TextAnchor.MiddleCenter;

            window.WindowConfig.VersionTextStyle = new GUIStyle();
            window.WindowConfig.VersionTextStyle.fontSize = 20;
            window.WindowConfig.VersionTextStyle.normal.textColor = Color.white;
            window.WindowConfig.VersionTextStyle.alignment = TextAnchor.MiddleRight;

            //¼ÓÔØÍ¼Æ¬×ÊÔ´µ½±à¼­Æ÷´°¿ÚÖĞ
            window.WindowConfig.LogoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ootw.jpg");
            window.WindowConfig.LogoTextureStyle = new GUIStyle();
            window.WindowConfig.LogoTextureStyle.alignment = TextAnchor.MiddleCenter;

        }
    }
    public static void LoadConfigFromJson()
    {
        //TextAsset configTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/AssetManagerConfig.amc");
        string configPath= Path.Combine(Application.dataPath, "Editor/AssetManagerConfig.amc");

        string configString = File.ReadAllText(configPath);

        JsonUtility.FromJsonOverwrite(configString,AssetManagerConfig);

    }

    public static void SaveConfigToJson()
    {
        if (AssetManagerConfig != null)
        {
            string configString = JsonUtility.ToJson(AssetManagerConfig);
            string outputPath = Path.Combine(Application.dataPath, "Editor/AssetManagerConfig.amc");

            File.WriteAllText(outputPath, configString);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }    

    /// <summary>
    /// ·µ»ØÓÉÒ»¸ö°üÖĞËùÓĞAssetµÄGUIDÁĞ±í¾­¹ıËã·¨¼ÓÃÜºóµÃµ½µÄ¹şÏ£Âë×Ö·û´®
    /// Èç¹ûGUIDÁĞ±í²»·¢Éú±ä»¯£¬ÒÔ¼°¼ÓÃÜËã·¨ºÍ²ÎÊıÃ»ÓĞ·¢Éú±ä»¯
    /// ÄÇÃ´×ÜÊÇÄÜ¹»µÃµ½ÏàÍ¬µÄ×Ö·û´®
    /// </summary>
    /// <param name="assetNames"></param>
    /// <returns></returns>
    static string ComputeAssetSetSignature(IEnumerable<string> assetNames)
    {
        var assetGUIDs = assetNames.Select(AssetDatabase.AssetPathToGUID);
        MD5 currentMD5 = MD5.Create();

        foreach(var assetGUID in assetGUIDs.OrderBy(x=>x))
        {
            byte[] bytes = Encoding.ASCII.GetBytes(assetGUID);
            //Ê¹ÓÃMD5Ëã·¨¼ÓÃÜ×Ö½ÚÊı×é
            currentMD5.TransformBlock(bytes, 0, bytes.Length, null, 0);
        }
        currentMD5.TransformFinalBlock(new byte[0], 0, 0);
        return BytesToHexString(currentMD5.Hash);
    }
    /// <summary>
    /// byte×ª16½øÖÆ×Ö·û´®
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    static string BytesToHexString(byte[] bytes)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach(var aByte in bytes)
        {
            stringBuilder.Append(aByte.ToString("x2"));
        }
        return stringBuilder.ToString();
    }

    static string[] BuildAssetBundleHashTable(AssetBundleBuild[] assetBundleBuilds)
    {
        //±íµÄ³¤¶ÈºÍAssetBundleµÄÊıÁ¿±£³ÖÒ»ÖÂ
        string[] assetBundleHashs = new string[assetBundleBuilds.Length];

        for(int i =0;i<assetBundleBuilds.Length;i++)
        {
            string assetBundlePath = Path.Combine(AssetBundleOutputPath, assetBundleBuilds[i].assetBundleName);
            FileInfo info = new FileInfo(assetBundlePath);
            //±íÖĞ¼ÇÂ¼µÄÊÇÒ»¸öAssetBundleÎÄ¼şµÄ³¤¶È£¬ÒÔ¼°ÆäÄÚÈİµÄMD5¹şÏ£Öµ
            assetBundleHashs[i] = $"{info.Length}_{assetBundleBuilds[i].assetBundleName}";
        }

        string hashString = JsonConvert.SerializeObject(assetBundleHashs);
        string hashFilePath = Path.Combine(AssetBundleOutputPath, "AssetBundleHashs");

        File.WriteAllText(hashFilePath, hashString);

        return assetBundleHashs;
    }

    static AssetBundleVersionDifference ContrastAssetBundleVersion(string[] oldVersionAssets,string[] newVersionAssets)
    {
        AssetBundleVersionDifference difference = new AssetBundleVersionDifference();
        foreach(var assetName in oldVersionAssets)
        {
            if(newVersionAssets.Contains(assetName))
            {
                difference.ReducedAssetBundles.Add(assetName);
            }
        }

        foreach(var assetName in newVersionAssets)
        {
            if(!oldVersionAssets.Contains(assetName))
            {
                difference.AdditionAssetBundles.Add(assetName);
            }
        }

        return difference;
    }

    public static void BuildAssetBundleFromDirectedGraph()
    {
        CheckBuildOutputPath();
        List<string> selectedAssets = GetAllSelectedAssets();
        List<AssetBundleNode> allNodes = new List<AssetBundleNode>();
        //µ±Ç°ËùÑ¡ÖĞµÄ×ÊÔ´¾ÍÊÇSourceAsset£¬ËùÒÔÊ×ÏÈµ÷¼ÓSourceAssetµÄNode
        for (int i = 0; i < selectedAssets.Count; i++)
        {
            AssetBundleNode currenNode = new AssetBundleNode();
            currenNode.AssetName = selectedAssets[i];
            currenNode.SourceIndex = i;
            currenNode.SourceIndeices = new List<int>() { i };
            currenNode.InEdge = new AssetBundleEdge();
            allNodes.Add(currenNode);

            GetNodeFromDependencies(currenNode, allNodes);
        }

        Dictionary<List<int>, List<AssetBundleNode>> assetBundleNodeDic = new Dictionary<List<int>, List<AssetBundleNode>>();
        foreach (AssetBundleNode node in allNodes)
        {
            bool isEquals = false;
            List<int> keyList = new List<int>();
            foreach (List<int> key in assetBundleNodeDic.Keys)
            {
                //ÅĞ¶ÏkeyµÄ³¤¶ÈÊÇ·ñºÍµ±Ç°nodeµÄSourceIndeies³¤¶ÈÏàµÈ
                isEquals = node.SourceIndeices.Count == key.Count && node.SourceIndeices.All(p => key.Any(k => k.Equals(p)));
                if (isEquals)
                {
                    keyList = key;
                    break;
                }
            }
            if (!isEquals)
            {
                keyList = node.SourceIndeices;
                assetBundleNodeDic.Add(node.SourceIndeices, new List<AssetBundleNode>());
            }
            assetBundleNodeDic[keyList].Add(node);
        }
        AssetBundleBuild[] assetBundleBuilds = new AssetBundleBuild[assetBundleNodeDic.Count];
        int buildIndex = 0;

        foreach (List<int> key in assetBundleNodeDic.Keys)
        {
            
            List<string> assetNames = new List<string>();
            //ÕâÒ»²ãÑ­»·¶¼ÊÇ´ÓÒ»¸ö¼üÖµ¶ÔÖĞ»ñÈ¡node
            //Ò²¾ÍÊÇ´ÓSourceIndeicesÏàÍ¬µÄ¼¯ºÏÖĞ»ñÈ¡ÏàÓ¦µÄNodeËù´ú±íµÄAsset
            foreach (AssetBundleNode node in assetBundleNodeDic[key])
            {
                assetNames.Add(node.AssetName);
            }
            string[] assetNamesArray = assetNames.ToArray();
            assetBundleBuilds[buildIndex].assetBundleName = ComputeAssetSetSignature(assetNamesArray);
            assetBundleBuilds[buildIndex].assetNames = assetNamesArray;
            buildIndex++;
        }
        BuildPipeline.BuildAssetBundles(AssetBundleOutputPath, assetBundleBuilds, CheckIncrementalBuildMode(),
            BuildTarget.StandaloneWindows);

        string[] currentVersionAssetHashs= BuildAssetBundleHashTable(assetBundleBuilds);

        CopyAssetBundleToVersionFolder();
        GetVersionDifference(currentVersionAssetHashs);
        AssetManagerConfig.CurrentBuildVersion++;
        //Ë¢ĞÂProject½çÃæ£¬Èç¹û²»ÊÇ´ò°üµ½¹¤³ÌÄÚÔò²»ĞèÒªÖ´ĞĞ
        AssetDatabase.Refresh();

        /*
        foreach(AssetBundleNode node in allNodes)
        {
            if(node.SourceIndex>=0)
            {
                Debug.Log($"{ node.AssetName}ÊÇÒ»¸öSourceAsset");
            }
            else
            {
                Debug.Log($"{ node.AssetName}ÊÇÒ»¸öDrivedAsset,±»{node.SourceIndeices.Count}¸ö×ÊÔ´ËùÒıÓÃ");
            }
        }*/

    }

    static void CopyAssetBundleToVersionFolder()
    {
        string versionString = AssetManagerConfig.CurrentBuildVersion.ToString();
        for (int i = versionString.Length - 1; i >= 1; i--)
        {
            versionString = versionString.Insert(i, ".");
        }

        string assetBundleVersionPath = Path.Combine(Application.streamingAssetsPath, versionString, HelloWorld.MainAssetBundleName);
        if(!Directory.Exists(assetBundleVersionPath))
        {
            Directory.CreateDirectory(assetBundleVersionPath);
        }

        string[] assetNames = ReadAssetBundleHashTable(AssetBundleOutputPath);

        //¸´ÖÆ¹şÏ£±í
        string hashTableOriginPath = Path.Combine(AssetBundleOutputPath, "AssetBundleHashs");
        string hashTableVersionPath = Path.Combine(assetBundleVersionPath, "AssetBundleHashs");
        File.Copy(hashTableOriginPath, hashTableVersionPath);
        //¸´ÖÆÖ÷°ü
        string mainBundleOriginPath = Path.Combine(AssetBundleOutputPath, HelloWorld.MainAssetBundleName);
        string mainBundleVersionPath = Path.Combine(assetBundleVersionPath, HelloWorld.MainAssetBundleName);
        File.Copy(mainBundleOriginPath, mainBundleVersionPath);

        foreach(var assetName in assetNames)
        {
            string assetHashName = assetName.Substring(assetName.IndexOf("_") + 1);

            string assetOriginPath = Path.Combine(AssetBundleOutputPath, assetHashName);
            //fileInfo.NameÊÇ°üº¬ÁËÀ©Õ¹ÃûµÄÎÄ¼şÃû
            string assetVersionPath = Path.Combine(assetBundleVersionPath, assetHashName);
            //fileInfo.FullNameÊÇ°üº¬ÁËÄ¿Â¼ºÍÎÄ¼şÃûµÄÎÄ¼şÍêÕûÂ·¾¶
            File.Copy(assetOriginPath, assetVersionPath,true);
        }
    }
    static BuildAssetBundleOptions CheckIncrementalBuildMode()
    {
        BuildAssetBundleOptions option = BuildAssetBundleOptions.None;
        switch(AssetManagerConfig._IncrementalBuildMode)
        {
            case IncrementalBuildMode.None:
                option = BuildAssetBundleOptions.None;
                break;
            case IncrementalBuildMode.IncrementalBuild:
                option = BuildAssetBundleOptions.DeterministicAssetBundle;
                break;
            case IncrementalBuildMode.ForceRebuild:
                option = BuildAssetBundleOptions.ForceRebuildAssetBundle;
                break;
        }
        return option;
    }

    static string[] ReadAssetBundleHashTable(string outputPath)
    {
        string VersionHashTablePath = Path.Combine(outputPath, "AssetBundleHashs");

        string VersionHashString = File.ReadAllText(VersionHashTablePath);

        string[] VersionAssetHashs = JsonConvert.DeserializeObject<string[]>(VersionHashString);

        return VersionAssetHashs;
    }

    static void GetVersionDifference(string[] currentAssetHashs)
    {
        if (AssetManagerConfig.CurrentBuildVersion >= 101)
        {
            int lastVersion = AssetManagerConfig.CurrentBuildVersion - 1;
            string versionString = AssetManagerConfig.CurrentBuildVersion.ToString();
            for (int i = versionString.Length - 1; i >= 1; i--)
            {
                versionString = versionString.Insert(i, ".");
            }
            var lastOutputPath = Path.Combine(Application.streamingAssetsPath, versionString, HelloWorld.MainAssetBundleName);

            string[] lastVersionAssetHashs = ReadAssetBundleHashTable(lastOutputPath);

            AssetBundleVersionDifference difference = ContrastAssetBundleVersion(lastVersionAssetHashs, currentAssetHashs);
            
            foreach (var assetName in difference.AdditionAssetBundles)
            {
                Debug.Log($"µ±Ç°°æ±¾ĞÂÔö×ÊÔ´{assetName}");
            }
            foreach (var assetName in difference.AdditionAssetBundles)
            {
                Debug.Log($"µ±Ç°°æ±¾¼õÉÙ×ÊÔ´{assetName}");
            }
            Debug.Log("1111");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lastNode"></param>µ÷ÓÃ¸Ãº¯ÊıµÄNode£¬±¾´Î´´½¨µÄËùÓĞNode¶¼Îª¸ÃNodeµÄOutEdge
    /// <param name="allNode"></param>µ±Ç°ËùÓĞµÄNode£¬¿ÉÒÔÓÃ³ÉÔ±±äÁ¿´úÌæ
    public static void GetNodeFromDependencies(AssetBundleNode lastNode, List<AssetBundleNode> allNodes)
    {
        //ÒòÎªÓĞÏòÍ¼ÊÇÒ»²ãÒ»²ã½¨ÒéÒÀÀµ¹ØÏµ£¬ËùÒÔ²»ÄÜÖ±½Ó»ñÈ¡µ±Ç°×ÊÔ´µÄÈ«²¿ÒÀÀµ
        //ËùÒÔÕâÀïÖ»»ñÈ¡µ±Ç°×ÊÔ´µÄÖ±½ÓÒÀÀµ
        string[] assetNames = AssetDatabase.GetDependencies(lastNode.AssetName, false);
        if (assetNames.Length == 0)
        {
            //ÓĞÏòÍ¼µ½ÁËÖÕµã
            return;
        }
        if (lastNode.OutEdge == null)
        {
            lastNode.OutEdge = new AssetBundleEdge();
        }
        foreach (string assetName in assetNames)
        {
            if (!isValidExtensionName(assetName))
            {
                continue;
            }
            AssetBundleNode currentNode = null;
            foreach (AssetBundleNode existingNode in allNodes)
            {
                //Èç¹ûµ±Ç°×ÊÔ´Ãû³ÆÒÑ¾­±»Ä³¸öNodeËùÊ¹ÓÃ£¬ÄÇÃ´ÅĞ¶ÏÏàÍ¬µÄ×ÊÔ´Ö±½ÓÊ¹ÓÃÒÑ¾­´æÔÚµÄNode
                if (existingNode.AssetName == assetName)
                {
                    currentNode = existingNode;
                    break;
                }
            }
            if (currentNode == null)
            {
                currentNode = new AssetBundleNode();
                currentNode.AssetName = assetName;
                currentNode.InEdge = new AssetBundleEdge();
                allNodes.Add(currentNode);
            }

            currentNode.InEdge.nodes.Add(lastNode);
            lastNode.OutEdge.nodes.Add(currentNode);

            //Èç¹ûlastNodeÊÇSourceAsset,ÔòÖ±½ÓÎªµ±Ç°NodeÌí¼Ólast NodeµÄIndex
            //ÒòÎªListÊÇÒ»¸öÒıÓÃÀàĞÍ£¬ËùÒÔSourceAssetµÄSourceindeiesÄÄÅÂÄÚÈİºÍderivedÒ»Ñù£¬Ò²ÊÓÎªÒ»¸öĞÂµÄList
            if (lastNode.SourceIndex >= 0)
            {

                currentNode.SourceIndeices.Add(lastNode.SourceIndex);
            }
            //·ñÔòÊÇDerivedAsset,Ö±½Ó»ñÈ¡last NodeµÄSourceIndices¼´¿É
            else
            {
                foreach (int index in lastNode.SourceIndeices)
                {
                    if (currentNode.SourceIndeices.Contains(index))
                    {
                        currentNode.SourceIndeices.Add(index);
                    }
                }
                currentNode.SourceIndeices = lastNode.SourceIndeices;
            }

        }
    }
    public static List<string> GetAllSelectedAssets()
    {
        List<string> selectedAssets = new List<string>();

        if (AssetManagerConfig.CurrentSelectedAssets == null || AssetManagerConfig.CurrentSelectedAssets.Length == 0)
        {
            return null;
        }
        //½«ÖµÎªtrueµÄ¶ÔÓ¦Ë÷ÒıÎÄ¼ş£¬Ìí¼Óµ½Òª´ò°üµÄ×ÊÔ´ÁĞ±íÖĞ
        for (int i = 0; i < AssetManagerConfig.CurrentSelectedAssets.Length; i++)
        {
            if (AssetManagerConfig.CurrentSelectedAssets[i])
            {
                selectedAssets.Add(AssetManagerConfig.CurrentAllAssets[i]);
            }
        }
        return selectedAssets;
    }

    public static List<string> GetSeletedAssetsDependencies()
    {
        List<string> depensencies = new List<string>();
        List<string> selecedAssets = GetAllSelectedAssets();
        for (int i = 0; i < selecedAssets.Count; i++)
        {
            //ËùÓĞÍ¨¹ı¸Ã·½·¨»ñÈ¡µ½µÄÊı×é£¬¿ÉÒÔÊÓÎª¼¯ºÏLÖĞµÄÒ»¸öÔªËØ
            string[] deps = AssetDatabase.GetDependencies(selecedAssets[i], true);
            foreach (string depName in deps)
            {
                Debug.Log(depName);
            }
        }
        return depensencies;
    }

    

    static BuildAssetBundleOptions CheckCompressionPattern()
    {
        BuildAssetBundleOptions option = new BuildAssetBundleOptions();
        switch (AssetManagerConfig.CompressionPattern)
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
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
        //·½·¨Ò»,Í¨¹ıEditorWindow.GetWindowWithRect()»ñÈ¡Ò»¸ö¾ßÓĞ¾ßÌå¾ØĞÎ´óĞ¡µÄ´°¿ÚÀà
        //Rect windowRect = new Rect(0, 0, 500, 500);
        //AssetManagerEditorWindow window = (AssetManagerEditorWindow) EditorWindow.GetWindowWithRect(typeof
        //    (AssetManagerEditorWindow),windowRect,true,nameof(AssetManagerEditor));

        //·½·¨¶ş£¬Í¨¹ıEditorWindow.GetWindow()»ñÈ¡Ò»¸ö×Ô¶¨Òå´óĞ¡£¬¿ÉÈÎÒâÍÏ×§µÄ´°¿Ú
        //AssetManagerEditorWindow window = (AssetManagerEditorWindow)EditorWindow.GetWindow(typeof
        //    (AssetManagerEditorWindow), true, nameof(AssetManagerEditor));
        //Èç¹û²»¸³ÓèÃû³Æ¾Í¿ÉÒÔ×÷ÎªUnity´°¿ÚËæÒâ·ÅÖÃÔÚÃæ°åÖĞ
        AssetManagerEditorWindow window = (AssetManagerEditorWindow)EditorWindow.GetWindow(typeof(AssetManagerEditorWindow));

    }

    static void CheckBuildOutputPath()
    {
<<<<<<< HEAD
        switch(BuildingPattern)
=======
        
        switch (AssetManagerConfig.BuildingPattern)
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
        {
            case AssetBundlePattern.EditorSimulation:
                break;
            case AssetBundlePattern.Local:
<<<<<<< HEAD
                 AssetBundleOutputPath = Path.Combine(Application.streamingAssetsPath, HelloWorld.MainAssetBundleName);
                break;
            case AssetBundlePattern.Remote:
                AssetBundleOutputPath = Path.Combine(Application.persistentDataPath, HelloWorld.MainAssetBundleName);
                break;
        }
    }

    /// <summary>
    /// ´ò°üÖ¸¶¨ÎÄ¼ş¼ĞÏÂËùÓĞ×ÊÔ´ÎªAssetBundle
    /// </summary>
    public static void BuildAssetBundleFromDirectory()
    {
        CheckBuildOutputPath();
        if (AssetBundleDirectory == null)
        {
            Debug.LogError("´ò°üÄ¿Â¼²»´æÔÚ");
            return;
        }
        //»ñÈ¡ÎÄ¼şÂ·¾¶
        string directoryPath = AssetDatabase.GetAssetPath(AssetBundleDirectory);
        //½«ÁĞ±í×ª»¯ÎªÊı×é
        string[] assetPaths = FindAllAssetFromDirectory(directoryPath).ToArray();

        AssetBundleBuild[] assetBundleBuild = new AssetBundleBuild[1];

        //½«Òª´ò°üµÄ¾ßÌå°üÃû£¬¶ø²»ÊÇÖ÷°üÃû
        assetBundleBuild[0].assetBundleName = HelloWorld.ObjectAssetBundleName;

        //ÕâÀïËäÈ»ÃûÎªName£¬Êµ¼ÊÉÏĞèÒª×ÊÔ´ÔÚ¹¤³ÌÏÂµÄÂ·¾¶
        assetBundleBuild[0].assetNames = assetPaths;

=======
                AssetBundleOutputPath = Path.Combine(Application.streamingAssetsPath,"Local", HelloWorld.MainAssetBundleName);
                break;
            case AssetBundlePattern.Remote:
                AssetBundleOutputPath = Path.Combine(Application.persistentDataPath, "Remote",HelloWorld.MainAssetBundleName);
                break;
        }
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
        if (string.IsNullOrEmpty(AssetBundleOutputPath))
        {
            Debug.LogError("Êä³öÂ·¾¶Îª¿Õ");
            return;
        }
        else if (!Directory.Exists(AssetBundleOutputPath))
        {
            //ÈôÂ·¾¶²»´æÔÚ¾Í´´½¨Â·¾¶
            Directory.CreateDirectory(AssetBundleOutputPath);
        }
<<<<<<< HEAD
        //UnityÖĞInspectorÃæ°åÅäÖÃµÄAssetBundleĞÅÏ¢£¬ÆäÊµ¾ÍÊÇÒ»¸öAssetBundle½á¹¹Ìå
        //UnityÖ»²»¹ıÊÇ±éÀúÁËËùÓĞµÄÎÄ¼ş¼Ğ£¬²¢°ÑÎÒÃÇÔÚÎÄ¼ş¼ĞÖĞÅäÖÃµÄAssetBundleÊÕ¼¯ÆğÀ´²¢´ò°ü
        BuildPipeline.BuildAssetBundles(AssetBundleOutputPath, assetBundleBuild, CheckCompressionPattern(),
=======
    }

    /// <summary>
    /// ÒòÎªListÊÇÒıÓÃĞÍ²ÎÊı£¬ËùÒÔ·½·¨ÖĞ¶ÔÓÚ²ÎÊıµÄĞŞ¸Ä»á·´Ó¦µ½´«Èë²ÎÊıµÄ±äÁ¿ÉÏ
    /// ÒòÎª±¾ÖÊÉÏ£¬²ÎÊıÖ»ÊÇÒıÓÃÁË±äÁ¿µÄÖ¸Õë£¬ËùÒÔ×îÖÕ»ãĞŞ¸ÄµÄÊÇÍ¬Ò»¸ö¶ÔÏóµÄÖµ
    /// </summary>
    /// <param name="setsA"></param>
    /// <param name="setsB"></param>
    /// <returns></returns>

    public static List<GUID> ContrastDepedenciesFromGUID(List<GUID> setsA, List<GUID> setsB)
    {
        List<GUID> newDependencies = new List<GUID>();
        //È¡½»¼¯
        foreach (var assetGUID in setsA)
        {
            if (setsB.Contains(assetGUID))
            {
                newDependencies.Add(assetGUID);
            }
        }
        //È¡²î¼¯
        foreach (var assetGUID in newDependencies)
        {
            if (setsA.Contains(assetGUID))
            {
                setsA.Remove(assetGUID);
            }
            if (setsB.Contains(assetGUID))
            {
                setsB.Remove(assetGUID);
            }
        }
        //·µ»Ø¼¯ºÏSnew
        return newDependencies;
    }
    public static void BuiAssetBundleFromSets()
    {
        CheckBuildOutputPath();
        if (AssetManagerConfig.AssetBundleDirectory == null)
        {
            Debug.LogError("´ò°üÄ¿Â¼²»´æÔÚ");
            return;
        }
        //±»Ñ¡ÖĞ½«Òª´ò°üµÄ×ÊÔ´ÁĞ±í,¼´ÁĞ±íA
        List<string> selectedAssets = GetAllSelectedAssets();

        //¼¯ºÏÁĞ±íL
        List<List<GUID>> selectedAssetsDependencies = new List<List<GUID>>();

        //±éÀúËùÓĞÑ¡ÔñµÄSourceAssetsÒÔ¼°ÒÀÀµ£¬»ñµÃ¼¯ºÏL
        foreach (string selectedAsset in selectedAssets)
        {
            //»ñÈ¡ËùÓĞSourceAssetµÄDerivedAsset
            string[] assetDeps = AssetDatabase.GetDependencies(selectedAsset, true);
            List<GUID> assetGUIDs = new List<GUID>();
            foreach (string assetdep in assetDeps)
            {
                GUID assetGUID = AssetDatabase.GUIDFromAssetPath(assetdep);
                assetGUIDs.Add(assetGUID);
            }

            //½«°üº¬ÁËSourceAssetÒÔ¼°DerivedAssetµÄ¼¯ºÏÌí¼Óµ½¼¯ºÏLÖĞ
            selectedAssetsDependencies.Add(assetGUIDs);
        }
        for (int i = 0; i < selectedAssetsDependencies.Count; i++)
        {
            int nextIndex = i + 1;
            if (nextIndex >= selectedAssetsDependencies.Count)
            {
                break;
            }
            Debug.Log($"¶Ô±ÈÖ®Ç°{selectedAssetsDependencies[i].Count}");
            Debug.Log($"¶Ô±ÈÖ®Ç°{selectedAssetsDependencies[nextIndex].Count}");

            for (int j = 0; j <= i; j++)
            {
                List<GUID> newDependencies = ContrastDepedenciesFromGUID(selectedAssetsDependencies[j], selectedAssetsDependencies[nextIndex]);
                //½«Snew¼¯ºÏÌí¼Óµ½¼¯ºÏÁĞ±íLÖĞ
                if (newDependencies != null && newDependencies.Count > 0)
                {
                    selectedAssetsDependencies.Add(newDependencies);
                }
            }
        }
        AssetBundleBuild[] assetBundleBuilds = new AssetBundleBuild[selectedAssetsDependencies.Count];
        for (int i = 0; i < assetBundleBuilds.Length; i++)
        {
            assetBundleBuilds[i].assetBundleName = i.ToString();
            string[] assetNames = new string[selectedAssetsDependencies[i].Count];
            List<GUID> assetGUIDs = selectedAssetsDependencies[i];
            for (int j = 0; j < assetNames.Length; j++)
            {
                string assetName = AssetDatabase.GUIDToAssetPath(assetGUIDs[j]);
                if (assetName.Contains(".cs"))
                {
                    continue;
                }
                assetNames[j] = assetName;
            }
            assetBundleBuilds[i].assetNames = assetNames;
        }

        /*
        BuildPipeline.BuildAssetBundles(AssetBundleOutputPath, assetBundleBuilds, CheckCompressionPattern(),
            BuildTarget.StandaloneWindows);

        //Ë¢ĞÂProject½çÃæ£¬Èç¹û²»ÊÇ´ò°üµ½¹¤³ÌÄÚÔò²»ĞèÒªÖ´ĞĞ
        AssetDatabase.Refresh();
        */
    }
    public static void BuildAssetBundleFromEditorWindow()
    {
        CheckBuildOutputPath();
        if (AssetManagerConfig.AssetBundleDirectory == null)
        {
            Debug.LogError("´ò°üÄ¿Â¼²»´æÔÚ");
            return;
        }

        //±»Ñ¡ÖĞ½«Òª´ò°üµÄ×ÊÔ´ÁĞ±í
        List<string> selectedAssets = GetAllSelectedAssets();

        //Ñ¡ÖĞ¶àÉÙ¸ö×ÊÔ´Ôò´ò°ü¶àÉÙ¸öAB°ü
        AssetBundleBuild[] assetBundleBuilds = new AssetBundleBuild[selectedAssets.Count];

        string directoryPath = AssetDatabase.GetAssetPath(AssetManagerConfig.AssetBundleDirectory);

        for (int i = 0; i < assetBundleBuilds.Length; i++)
        {
            string bundleName = selectedAssets[i].Replace($@"{directoryPath}\", string.Empty);
            //Unity×÷µ¼Èë.prefabÎÄ×÷Ê±£¬»áÄ¬ÈÏÊ¹ÓÃÔ¤ÖÆÌåµ¼ÈëÆ÷µ¼Èë£¬¶øassetBundle²»ÊÇÔ¤ÖÆÌå£¬ËùÒÔ»áµ¼ÖÂ±¨´í
            bundleName = bundleName.Replace(".prefab", string.Empty);

            assetBundleBuilds[i].assetBundleName = bundleName;

            assetBundleBuilds[i].assetNames = new string[] { selectedAssets[i] };
        }

        BuildPipeline.BuildAssetBundles(AssetBundleOutputPath, assetBundleBuilds, CheckCompressionPattern(),
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
            BuildTarget.StandaloneWindows);

        //´òÓ¡Êä³öÂ·¾¶
        Debug.Log(AssetBundleOutputPath);

        //Ë¢ĞÂProject½çÃæ£¬Èç¹û²»ÊÇ´ò°üµ½¹¤³ÌÄÚÔò²»ĞèÒªÖ´ĞĞ
        AssetDatabase.Refresh();
    }

<<<<<<< HEAD
    public static List<string> FindAllAssetFromDirectory(string directoryPath)
    {
        List<string> assetPaths = new List<string>();
        //Èç¹û´«ÈëµÄÂ·¾¶Îª¿Õ»òÕß²»´æÔÚµÄ»°
        if(string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
=======
    /// <summary>
    /// ´ò°üÖ¸¶¨ÎÄ¼ş¼ĞÏÂËùÓĞ×ÊÔ´ÎªAssetBundle
    /// </summary>
    public static void BuildAssetBundleFromDirectory()
    {
        CheckBuildOutputPath();
        if (AssetManagerConfig.AssetBundleDirectory == null)
        {
            Debug.LogError("´ò°üÄ¿Â¼²»´æÔÚ");
            return;
        }


        AssetBundleBuild[] assetBundleBuild = new AssetBundleBuild[1];

        //½«Òª´ò°üµÄ¾ßÌå°üÃû£¬¶ø²»ÊÇÖ÷°üÃû
        assetBundleBuild[0].assetBundleName = HelloWorld.ObjectAssetBundleName;

        //ÕâÀïËäÈ»ÃûÎªName£¬Êµ¼ÊÉÏĞèÒª×ÊÔ´ÔÚ¹¤³ÌÏÂµÄÂ·¾¶
        assetBundleBuild[0].assetNames = AssetManagerConfig.CurrentAllAssets.ToArray();

    }

    /// <summary>
    /// ´«Èë°üº¬À©Õ¹ÃûµÄÎÄ¼şÃû£¬ÓÃÓÚºÍÎŞĞ§ÍØÕ¹ÃûÊı×é½øĞĞ¶Ô±È
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool isValidExtensionName(string fileName)
    {
        bool isValid = true;
        foreach (string invalidName in AssetManagerConfig.InvalidExtensionNames)
        {
            if (fileName.Contains(invalidName))
            {
                isValid = false;
                return isValid;
            }
        }
        return isValid;
    }

    public List<string> FindAllAssetFromDirectory(string directoryPath)
    {
        List<string> assetPaths = new List<string>();
        //Èç¹û´«ÈëµÄÂ·¾¶Îª¿Õ»òÕß²»´æÔÚµÄ»°
        if (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
        {
            Debug.Log("ÎÄ¼ş¼ĞÂ·¾¶²»´æÔÚ");
            return null;
        }
        //System.IOÃüÃû¿Õ¼äÏÂµÄÀà£¬Ò²¾ÍÊÇWindows×Ô´øµÄ¶ÔÎÄ¼ş¼Ğ½øĞĞ²Ù×÷µÄÀà
        //System.IOÏÂµÄÀà£¬Ö»ÄÜÔÚPCÆ½Ì¨»òÕßWindowsÉÏ¶ÁĞ´ÎÄ¼ş£¬ÔÚÒÆ¶¯¶Ë²»ÊÊÓÃ
        DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

        //»ñÈ¡¸ÃÄ¿Â¼ÏÂËùÓĞÎÄ¼şĞÅÏ¢
        //DirectoryÎÄ¼ş¼Ğ²»ÊôÓÚFileÀàĞÍ£¬ËùÒÔÕâÀï²»»á»ñÈ¡×ÓÎÄ¼ş¼Ğ
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        //ËùÓĞ·ÇÔªÊı¾İÎÄ¼ş(ºó×º²»ÊÇmetaµÄÎÄ¼ş)Â·¾¶¶¼Ìí¼Óµ½ÁĞ±íÖĞÓÃÓÚ´ò°üÕâĞ©ÎÄ¼ş
<<<<<<< HEAD
        foreach(FileInfo info in fileInfos)
        {
            //.metaÎÄ¼ş´ú±íÃèÊöÎÄ¼ş
            if(info.Extension.Contains(".meta"))
=======
        foreach (FileInfo info in fileInfos)
        {
            //.metaÎÄ¼ş´ú±íÃèÊöÎÄ¼ş
            if (!isValidExtensionName(info.Extension))
>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
            {
                continue;
            }
            //AssetBundle´ò°üÖ»ĞèÒªÎÄ¼şÃû
            string assetPath = Path.Combine(directoryPath, info.Name);
            assetPaths.Add(assetPath);
            Debug.Log(assetPath);
        }
        return assetPaths;
    }
<<<<<<< HEAD
=======

>>>>>>> d91a4b8 (ScriptableObjectçš„ä½¿ç”¨)
}