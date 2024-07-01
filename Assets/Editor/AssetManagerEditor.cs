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
>>>>>>> d91a4b8 (ScriptableObject的使用)

public enum AssetBundleCompressionPattern
{
    LZMA,
    LZ4,
    None
}

/// <summary>
<<<<<<< HEAD
/// ������EditorĿ¼�µ�C#�ű������������Դ�������ִ���ļ�������
/// </summary>
public class AssetManagerEditor 
{
    //�����汾��
    public static string AssetManagerVersion = "1.0.0";

    /// <summary>
    /// �༭��ģ���£������д��
    /// ����ģʽ�������StreamingAssets
    /// Զ��ģʽ�����������Զ��·�����ڸ�ʾ����ΪpersistentDataPath
    /// </summary>
    public static AssetBundlePattern BuildingPattern;

    public static AssetBundleCompressionPattern CompressionPattern;

    /// <summary>
    /// ��Ҫ������ļ���
    /// </summary>
    public static DefaultAsset AssetBundleDirectory;



    public static string AssetBundleOutputPath;

    /// <summary>
    /// ͨ��MenuItem���ԣ�����Editor�����˵���ѡ��
    /// </summary>
    [MenuItem(nameof(AssetManagerEditor)+"/"+nameof(BuildAssetBundle))]
=======
/// �κ�BuildOption���ڷ�forceRebuildѡ���¶�Ĭ��Ϊ�������
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
    /// ������Դ��
    /// </summary>
    public List<string> AdditionAssetBundles=new List<string>();
    /// <summary>
    /// ������Դ��
    /// </summary>
    public List<string> ReducedAssetBundles=new List<string>();
}

/// <summary>
///������Node֮������ù�ϵ������Ȼһ��Node֮��������ö��Node��Ҳ���ܱ����Node������
/// </summary>
public class AssetBundleEdge
{
    public List<AssetBundleNode> nodes = new List<AssetBundleNode>();
}

public class AssetBundleNode
{
    public string AssetName;
    /// <summary>
    /// ���������ж�һ����Դ�Ƿ���SourceAsset�������-1˵����DerivedAsset
    /// </summary>
    public int SourceIndex = -1;

    /// <summary>
    /// ��ǰNode��Index�б������������OutEdge���д���
    /// </summary>
    public List<int> SourceIndeices = new List<int>();
    /// <summary>
    /// ��ǰNode�����õ�Nodes
    /// </summary>
    public AssetBundleEdge OutEdge;
    /// <summary>
    /// ���õ�ǰNode��Nodes
    /// </summary>
    public AssetBundleEdge InEdge;
}


/// <summary>
/// ������EditorĿ¼�µ�C#�ű������������Դ�������ִ���ļ�������
/// </summary>
public class AssetManagerEditor
{
    public static AssetManagerConfigScriptableObject AssetManagerConfig;

    

    public static string AssetBundleOutputPath;


    /// <summary>
    /// ͨ��MenuItem���ԣ�����Editor�����˵���ѡ��
    /// </summary>
    [MenuItem(nameof(AssetManagerEditor) + "/" + nameof(BuildAssetBundle))]
>>>>>>> d91a4b8 (ScriptableObject的使用)
    static void BuildAssetBundle()
    {
        CheckBuildOutputPath();
        //PathCombine���������ڼ����ַ���֮�����б��
        //string outputPath = "E:/AssetBundles/testAB1";
        //string outputPath = "E:/AssetBundles/testAB2";
        //string outputPath = "E:/AssetBundles/testAB3";

        if (!Directory.Exists(AssetBundleOutputPath))
        {
            Directory.CreateDirectory(AssetBundleOutputPath);
        }

        //��ͬƽ̨֮���AssetBundle������ͨ��
        //�÷����������������������˰�����AB���������û�����ð����ʹ򲻳���
        //OptionsΪNoneʱʹ��LZMAѹ��
        //UncompressedAssetBundle������ѹ��
        //ChunkBasedCompression����LZ4��ѹ��

        BuildPipeline.BuildAssetBundles(AssetBundleOutputPath, CheckCompressionPattern(), EditorUserBuildSettings.activeBuildTarget);

        Debug.Log("AB����������");
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
        //����ScriptableObject���͵�ʵ��
        //ScriptableObject���͵�������������JSON�н�ĳ�����ʵ�����л��Ĺ���
        AssetManagerConfigScriptableObject config = ScriptableObject.CreateInstance<AssetManagerConfigScriptableObject>();

        AssetDatabase.CreateAsset(config, "Assets/Editor/AssetManagerConfig.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void LoadConfig(AssetManagerEditorWindow window)
    {
        if (AssetManagerConfig == null)
        {
            //ʹ��AssetDatabase������Դ��ֻ��Ҫ����AssetsĿ¼�µ�·������
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
            //ʹ��AssetDatabase������Դ��ֻ��Ҫ����AssetsĿ¼�µ�·������
            window.WindowConfig = AssetDatabase.LoadAssetAtPath<AssetManagerEditorWindowConfigSO>("Assets/Editor/AssetManagerEditorWindowConfig.asset");
            window.WindowConfig.TitleTextStyle = new GUIStyle();
            window.WindowConfig.TitleTextStyle.fontSize = 26;
            window.WindowConfig.TitleTextStyle.normal.textColor = Color.red;
            window.WindowConfig.TitleTextStyle.alignment = TextAnchor.MiddleCenter;

            window.WindowConfig.VersionTextStyle = new GUIStyle();
            window.WindowConfig.VersionTextStyle.fontSize = 20;
            window.WindowConfig.VersionTextStyle.normal.textColor = Color.white;
            window.WindowConfig.VersionTextStyle.alignment = TextAnchor.MiddleRight;

            //����ͼƬ��Դ���༭��������
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
    /// ������һ����������Asset��GUID�б����㷨���ܺ�õ��Ĺ�ϣ���ַ���
    /// ���GUID�б������仯���Լ������㷨�Ͳ���û�з����仯
    /// ��ô�����ܹ��õ���ͬ���ַ���
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
            //ʹ��MD5�㷨�����ֽ�����
            currentMD5.TransformBlock(bytes, 0, bytes.Length, null, 0);
        }
        currentMD5.TransformFinalBlock(new byte[0], 0, 0);
        return BytesToHexString(currentMD5.Hash);
    }
    /// <summary>
    /// byteת16�����ַ���
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
        //��ĳ��Ⱥ�AssetBundle����������һ��
        string[] assetBundleHashs = new string[assetBundleBuilds.Length];

        for(int i =0;i<assetBundleBuilds.Length;i++)
        {
            string assetBundlePath = Path.Combine(AssetBundleOutputPath, assetBundleBuilds[i].assetBundleName);
            FileInfo info = new FileInfo(assetBundlePath);
            //���м�¼����һ��AssetBundle�ļ��ĳ��ȣ��Լ������ݵ�MD5��ϣֵ
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
        //��ǰ��ѡ�е���Դ����SourceAsset���������ȵ���SourceAsset��Node
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
                //�ж�key�ĳ����Ƿ�͵�ǰnode��SourceIndeies�������
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
            //��һ��ѭ�����Ǵ�һ����ֵ���л�ȡnode
            //Ҳ���Ǵ�SourceIndeices��ͬ�ļ����л�ȡ��Ӧ��Node�������Asset
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
        //ˢ��Project���棬������Ǵ��������������Ҫִ��
        AssetDatabase.Refresh();

        /*
        foreach(AssetBundleNode node in allNodes)
        {
            if(node.SourceIndex>=0)
            {
                Debug.Log($"{ node.AssetName}��һ��SourceAsset");
            }
            else
            {
                Debug.Log($"{ node.AssetName}��һ��DrivedAsset,��{node.SourceIndeices.Count}����Դ������");
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

        //���ƹ�ϣ��
        string hashTableOriginPath = Path.Combine(AssetBundleOutputPath, "AssetBundleHashs");
        string hashTableVersionPath = Path.Combine(assetBundleVersionPath, "AssetBundleHashs");
        File.Copy(hashTableOriginPath, hashTableVersionPath);
        //��������
        string mainBundleOriginPath = Path.Combine(AssetBundleOutputPath, HelloWorld.MainAssetBundleName);
        string mainBundleVersionPath = Path.Combine(assetBundleVersionPath, HelloWorld.MainAssetBundleName);
        File.Copy(mainBundleOriginPath, mainBundleVersionPath);

        foreach(var assetName in assetNames)
        {
            string assetHashName = assetName.Substring(assetName.IndexOf("_") + 1);

            string assetOriginPath = Path.Combine(AssetBundleOutputPath, assetHashName);
            //fileInfo.Name�ǰ�������չ�����ļ���
            string assetVersionPath = Path.Combine(assetBundleVersionPath, assetHashName);
            //fileInfo.FullName�ǰ�����Ŀ¼���ļ������ļ�����·��
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
                Debug.Log($"��ǰ�汾������Դ{assetName}");
            }
            foreach (var assetName in difference.AdditionAssetBundles)
            {
                Debug.Log($"��ǰ�汾������Դ{assetName}");
            }
            Debug.Log("1111");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lastNode"></param>���øú�����Node�����δ���������Node��Ϊ��Node��OutEdge
    /// <param name="allNode"></param>��ǰ���е�Node�������ó�Ա��������
    public static void GetNodeFromDependencies(AssetBundleNode lastNode, List<AssetBundleNode> allNodes)
    {
        //��Ϊ����ͼ��һ��һ�㽨��������ϵ�����Բ���ֱ�ӻ�ȡ��ǰ��Դ��ȫ������
        //��������ֻ��ȡ��ǰ��Դ��ֱ������
        string[] assetNames = AssetDatabase.GetDependencies(lastNode.AssetName, false);
        if (assetNames.Length == 0)
        {
            //����ͼ�����յ�
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
                //�����ǰ��Դ�����Ѿ���ĳ��Node��ʹ�ã���ô�ж���ͬ����Դֱ��ʹ���Ѿ����ڵ�Node
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

            //���lastNode��SourceAsset,��ֱ��Ϊ��ǰNode���last Node��Index
            //��ΪList��һ���������ͣ�����SourceAsset��Sourceindeies�������ݺ�derivedһ����Ҳ��Ϊһ���µ�List
            if (lastNode.SourceIndex >= 0)
            {

                currentNode.SourceIndeices.Add(lastNode.SourceIndex);
            }
            //������DerivedAsset,ֱ�ӻ�ȡlast Node��SourceIndices����
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
        //��ֵΪtrue�Ķ�Ӧ�����ļ�����ӵ�Ҫ�������Դ�б���
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
            //����ͨ���÷�����ȡ�������飬������Ϊ����L�е�һ��Ԫ��
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
>>>>>>> d91a4b8 (ScriptableObject的使用)
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
        //����һ,ͨ��EditorWindow.GetWindowWithRect()��ȡһ�����о�����δ�С�Ĵ�����
        //Rect windowRect = new Rect(0, 0, 500, 500);
        //AssetManagerEditorWindow window = (AssetManagerEditorWindow) EditorWindow.GetWindowWithRect(typeof
        //    (AssetManagerEditorWindow),windowRect,true,nameof(AssetManagerEditor));

        //��������ͨ��EditorWindow.GetWindow()��ȡһ���Զ����С����������ק�Ĵ���
        //AssetManagerEditorWindow window = (AssetManagerEditorWindow)EditorWindow.GetWindow(typeof
        //    (AssetManagerEditorWindow), true, nameof(AssetManagerEditor));
        //������������ƾͿ�����ΪUnity������������������
        AssetManagerEditorWindow window = (AssetManagerEditorWindow)EditorWindow.GetWindow(typeof(AssetManagerEditorWindow));

    }

    static void CheckBuildOutputPath()
    {
<<<<<<< HEAD
        switch(BuildingPattern)
=======
        
        switch (AssetManagerConfig.BuildingPattern)
>>>>>>> d91a4b8 (ScriptableObject的使用)
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
    /// ���ָ���ļ�����������ԴΪAssetBundle
    /// </summary>
    public static void BuildAssetBundleFromDirectory()
    {
        CheckBuildOutputPath();
        if (AssetBundleDirectory == null)
        {
            Debug.LogError("���Ŀ¼������");
            return;
        }
        //��ȡ�ļ�·��
        string directoryPath = AssetDatabase.GetAssetPath(AssetBundleDirectory);
        //���б�ת��Ϊ����
        string[] assetPaths = FindAllAssetFromDirectory(directoryPath).ToArray();

        AssetBundleBuild[] assetBundleBuild = new AssetBundleBuild[1];

        //��Ҫ����ľ��������������������
        assetBundleBuild[0].assetBundleName = HelloWorld.ObjectAssetBundleName;

        //������Ȼ��ΪName��ʵ������Ҫ��Դ�ڹ����µ�·��
        assetBundleBuild[0].assetNames = assetPaths;

=======
                AssetBundleOutputPath = Path.Combine(Application.streamingAssetsPath,"Local", HelloWorld.MainAssetBundleName);
                break;
            case AssetBundlePattern.Remote:
                AssetBundleOutputPath = Path.Combine(Application.persistentDataPath, "Remote",HelloWorld.MainAssetBundleName);
                break;
        }
>>>>>>> d91a4b8 (ScriptableObject的使用)
        if (string.IsNullOrEmpty(AssetBundleOutputPath))
        {
            Debug.LogError("���·��Ϊ��");
            return;
        }
        else if (!Directory.Exists(AssetBundleOutputPath))
        {
            //��·�������ھʹ���·��
            Directory.CreateDirectory(AssetBundleOutputPath);
        }
<<<<<<< HEAD
        //Unity��Inspector������õ�AssetBundle��Ϣ����ʵ����һ��AssetBundle�ṹ��
        //Unityֻ�����Ǳ��������е��ļ��У������������ļ��������õ�AssetBundle�ռ����������
        BuildPipeline.BuildAssetBundles(AssetBundleOutputPath, assetBundleBuild, CheckCompressionPattern(),
=======
    }

    /// <summary>
    /// ��ΪList�������Ͳ��������Է����ж��ڲ������޸ĻᷴӦ����������ı�����
    /// ��Ϊ�����ϣ�����ֻ�������˱�����ָ�룬�������ջ��޸ĵ���ͬһ�������ֵ
    /// </summary>
    /// <param name="setsA"></param>
    /// <param name="setsB"></param>
    /// <returns></returns>

    public static List<GUID> ContrastDepedenciesFromGUID(List<GUID> setsA, List<GUID> setsB)
    {
        List<GUID> newDependencies = new List<GUID>();
        //ȡ����
        foreach (var assetGUID in setsA)
        {
            if (setsB.Contains(assetGUID))
            {
                newDependencies.Add(assetGUID);
            }
        }
        //ȡ�
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
        //���ؼ���Snew
        return newDependencies;
    }
    public static void BuiAssetBundleFromSets()
    {
        CheckBuildOutputPath();
        if (AssetManagerConfig.AssetBundleDirectory == null)
        {
            Debug.LogError("���Ŀ¼������");
            return;
        }
        //��ѡ�н�Ҫ�������Դ�б�,���б�A
        List<string> selectedAssets = GetAllSelectedAssets();

        //�����б�L
        List<List<GUID>> selectedAssetsDependencies = new List<List<GUID>>();

        //��������ѡ���SourceAssets�Լ���������ü���L
        foreach (string selectedAsset in selectedAssets)
        {
            //��ȡ����SourceAsset��DerivedAsset
            string[] assetDeps = AssetDatabase.GetDependencies(selectedAsset, true);
            List<GUID> assetGUIDs = new List<GUID>();
            foreach (string assetdep in assetDeps)
            {
                GUID assetGUID = AssetDatabase.GUIDFromAssetPath(assetdep);
                assetGUIDs.Add(assetGUID);
            }

            //��������SourceAsset�Լ�DerivedAsset�ļ�����ӵ�����L��
            selectedAssetsDependencies.Add(assetGUIDs);
        }
        for (int i = 0; i < selectedAssetsDependencies.Count; i++)
        {
            int nextIndex = i + 1;
            if (nextIndex >= selectedAssetsDependencies.Count)
            {
                break;
            }
            Debug.Log($"�Ա�֮ǰ{selectedAssetsDependencies[i].Count}");
            Debug.Log($"�Ա�֮ǰ{selectedAssetsDependencies[nextIndex].Count}");

            for (int j = 0; j <= i; j++)
            {
                List<GUID> newDependencies = ContrastDepedenciesFromGUID(selectedAssetsDependencies[j], selectedAssetsDependencies[nextIndex]);
                //��Snew������ӵ������б�L��
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

        //ˢ��Project���棬������Ǵ��������������Ҫִ��
        AssetDatabase.Refresh();
        */
    }
    public static void BuildAssetBundleFromEditorWindow()
    {
        CheckBuildOutputPath();
        if (AssetManagerConfig.AssetBundleDirectory == null)
        {
            Debug.LogError("���Ŀ¼������");
            return;
        }

        //��ѡ�н�Ҫ�������Դ�б�
        List<string> selectedAssets = GetAllSelectedAssets();

        //ѡ�ж��ٸ���Դ�������ٸ�AB��
        AssetBundleBuild[] assetBundleBuilds = new AssetBundleBuild[selectedAssets.Count];

        string directoryPath = AssetDatabase.GetAssetPath(AssetManagerConfig.AssetBundleDirectory);

        for (int i = 0; i < assetBundleBuilds.Length; i++)
        {
            string bundleName = selectedAssets[i].Replace($@"{directoryPath}\", string.Empty);
            //Unity������.prefab����ʱ����Ĭ��ʹ��Ԥ���嵼�������룬��assetBundle����Ԥ���壬���Իᵼ�±���
            bundleName = bundleName.Replace(".prefab", string.Empty);

            assetBundleBuilds[i].assetBundleName = bundleName;

            assetBundleBuilds[i].assetNames = new string[] { selectedAssets[i] };
        }

        BuildPipeline.BuildAssetBundles(AssetBundleOutputPath, assetBundleBuilds, CheckCompressionPattern(),
>>>>>>> d91a4b8 (ScriptableObject的使用)
            BuildTarget.StandaloneWindows);

        //��ӡ���·��
        Debug.Log(AssetBundleOutputPath);

        //ˢ��Project���棬������Ǵ��������������Ҫִ��
        AssetDatabase.Refresh();
    }

<<<<<<< HEAD
    public static List<string> FindAllAssetFromDirectory(string directoryPath)
    {
        List<string> assetPaths = new List<string>();
        //��������·��Ϊ�ջ��߲����ڵĻ�
        if(string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
=======
    /// <summary>
    /// ���ָ���ļ�����������ԴΪAssetBundle
    /// </summary>
    public static void BuildAssetBundleFromDirectory()
    {
        CheckBuildOutputPath();
        if (AssetManagerConfig.AssetBundleDirectory == null)
        {
            Debug.LogError("���Ŀ¼������");
            return;
        }


        AssetBundleBuild[] assetBundleBuild = new AssetBundleBuild[1];

        //��Ҫ����ľ��������������������
        assetBundleBuild[0].assetBundleName = HelloWorld.ObjectAssetBundleName;

        //������Ȼ��ΪName��ʵ������Ҫ��Դ�ڹ����µ�·��
        assetBundleBuild[0].assetNames = AssetManagerConfig.CurrentAllAssets.ToArray();

    }

    /// <summary>
    /// ���������չ�����ļ��������ں���Ч��չ��������жԱ�
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
        //��������·��Ϊ�ջ��߲����ڵĻ�
        if (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
>>>>>>> d91a4b8 (ScriptableObject的使用)
        {
            Debug.Log("�ļ���·��������");
            return null;
        }
        //System.IO�����ռ��µ��࣬Ҳ����Windows�Դ��Ķ��ļ��н��в�������
        //System.IO�µ��ֻ࣬����PCƽ̨����Windows�϶�д�ļ������ƶ��˲�����
        DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

        //��ȡ��Ŀ¼�������ļ���Ϣ
        //Directory�ļ��в�����File���ͣ��������ﲻ���ȡ���ļ���
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        //���з�Ԫ�����ļ�(��׺����meta���ļ�)·������ӵ��б������ڴ����Щ�ļ�
<<<<<<< HEAD
        foreach(FileInfo info in fileInfos)
        {
            //.meta�ļ����������ļ�
            if(info.Extension.Contains(".meta"))
=======
        foreach (FileInfo info in fileInfos)
        {
            //.meta�ļ����������ļ�
            if (!isValidExtensionName(info.Extension))
>>>>>>> d91a4b8 (ScriptableObject的使用)
            {
                continue;
            }
            //AssetBundle���ֻ��Ҫ�ļ���
            string assetPath = Path.Combine(directoryPath, info.Name);
            assetPaths.Add(assetPath);
            Debug.Log(assetPath);
        }
        return assetPaths;
    }
<<<<<<< HEAD
=======

>>>>>>> d91a4b8 (ScriptableObject的使用)
}