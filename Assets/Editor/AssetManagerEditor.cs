using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System;

/// <summary>
/// �����ֻ�����ֻ���Editor�����´��ڵ�Package��Ϣ
/// ���Ǵ��֮���Package��Ϣ
/// </summary>
[Serializable]
public class PackageEditorInfo
{
    /// <summary>
    /// ��ǰ��������
    /// �����ɿ������ڱ༭�����������ɶ���
    /// </summary>
    public string PackageName;

    /// <summary>
    /// �����ڵ�ǰ���е���Դ�б�
    /// �����ɿ������ڱ༭�����������ɶ���
    /// </summary>
    public List<UnityEngine.Object> AssetList = new List<UnityEngine.Object>();
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
    /// ֻ��SourceAsset�ž��а���
    /// </summary>
    public string PackageName;

    /// <summary>
    /// DerivedAsset��ֻ��PackageNames�������ù�ϵ
    /// </summary>
    public List<string> PackageNames = new List<string>();

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

    
    /// <summary>
    /// ���δ������AssetBundle�����·��
    /// Ӧ���������������������������
    /// </summary>
    public static string AssetBundleOutputPath;

    /// <summary>
    /// ��������������ļ����·��
    /// </summary>
    public static string BuildOutputPath;

    /// <summary>
    /// ͨ��MenuItem���ԣ�����Editor�����˵���ѡ��
    /// </summary>
    [MenuItem(nameof(AssetManagerEditor) + "/" + nameof(BuildAssetBundle))]
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

    public static void AddPackageInfoEditor()
    {
        AssetManagerConfig.packageEditorInfos.Add(new PackageEditorInfo());
    }

    public static void RomovePackageInfoEditor(PackageEditorInfo info)
    {
        if(AssetManagerConfig.packageEditorInfos.Contains(info))
        {
            AssetManagerConfig.packageEditorInfos.Remove(info);
        }
    }
    public static void AddAsset(PackageEditorInfo info)
    {
        info.AssetList.Add(null);
    }
    
    public static void RemoveAsset(PackageEditorInfo info,UnityEngine.Object asset)
    {
        if(info.AssetList.Contains(asset))
        {
            info.AssetList.Remove(asset);
        }
    }

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

            for(int i=window.VersionString.Length-1;i>=1;i--)
            {
                window.VersionString = window.VersionString.Insert(i, ".");
            }

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

    static string[] BuildAssetBundleHashTable(AssetBundleBuild[] assetBundleBuilds,string versionPath)
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
        string hashFileVersionPath = Path.Combine(versionPath, "AssetBundleHashs");

        File.WriteAllText(hashFilePath, hashString);
        File.WriteAllText(hashFileVersionPath, hashString);

        return assetBundleHashs;
    }

    
    public static void BuildAssetBundleFromDirectedGraph()
    {
        CheckBuildOutputPath();

        List<AssetBundleNode> allNodes = new List<AssetBundleNode>();

        int sourceIndex = 0;

        Dictionary<string, PackageBuildInfo> packageInfoDic = new Dictionary<string, PackageBuildInfo>();
        #region ����ͼ����
        for (int i=0;i<AssetManagerConfig.packageEditorInfos.Count; i++)
        {
            PackageBuildInfo packageBuildInfo = new PackageBuildInfo();
            packageBuildInfo.PackageName = AssetManagerConfig.packageEditorInfos[i].PackageName;

            packageBuildInfo.IsSourcePackage = true;

            packageInfoDic.Add(packageBuildInfo.PackageName, packageBuildInfo);

            //��ǰ��ѡ�е���Դ����SourceAsset���������ȵ���SourceAsset��Node
            foreach (UnityEngine.Object asset in AssetManagerConfig.packageEditorInfos[i].AssetList)
            {
                AssetBundleNode currenNode = null;
                //����Դ�ľ���·������Ϊ��Դ����
                string assetNamePath= AssetDatabase.GetAssetPath(asset);

                foreach(AssetBundleNode node in allNodes)
                {
                    if(node.AssetName==assetNamePath)
                    {
                        currenNode = node;
                        currenNode.PackageName = packageBuildInfo.PackageName;
                        break;
                    }
                }

                if(currenNode==null)
                {
                    currenNode = new AssetBundleNode();
                    currenNode.AssetName = assetNamePath;

                    currenNode.SourceIndex = sourceIndex;
                    currenNode.SourceIndeices = new List<int>() { sourceIndex };

                    currenNode.PackageName = packageBuildInfo.PackageName;
                    currenNode.PackageNames.Add(currenNode.PackageName);

                    currenNode.InEdge = new AssetBundleEdge();
                    allNodes.Add(currenNode);
                }

                GetNodeFromDependencies(currenNode, allNodes);

                sourceIndex++;
            }
        }
        #endregion

        #region ����ͼ���ִ������
        Dictionary<List<int>, List<AssetBundleNode>> assetBundleNodeDic = new Dictionary<List<int>, List<AssetBundleNode>>();

        foreach (AssetBundleNode node in allNodes)
        {
            StringBuilder packageNameString = new StringBuilder();

            //������Ϊ�ջ��ޣ��������һ��SourceAsset��������Ѿ��ڱ༭�������������
            if(string.IsNullOrEmpty(node.PackageName))
            {
                for(int i =0; i<node.PackageNames.Count;i++)
                {
                    packageNameString.Append(node.PackageNames[i]);
                    if(i<node.PackageNames.Count-1)
                    {
                        packageNameString.Append("_");
                    }
                }

                string packageName = packageNameString.ToString();
                node.PackageName = packageName;

                //��ʱֻ����˶�Ӧ�İ��Ͱ���
                //��û�о�����Ӱ��ж�Ӧ��Asset
                //��ΪAsset���ʱ��Ҫ����AssetBundleName,
                //����ֻ��������AssetBundleBuild�ĵط����Asset
                if(!packageInfoDic.ContainsKey(packageName))
                {
                    PackageBuildInfo packageBuildInfo = new PackageBuildInfo();
                    packageBuildInfo.PackageName = packageName;
                    packageBuildInfo.IsSourcePackage = false;
                    packageInfoDic.Add(packageBuildInfo.PackageName, packageBuildInfo);
                }
            }

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
        #endregion

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

                //�����һ��SourceAsset��������PackageNameֻ��������Լ�
                foreach (string packageName in node.PackageNames)
                {
                    if(packageInfoDic.ContainsKey(packageName))
                    {
                        if(!packageInfoDic[packageName].PackageDependencies.Contains(node.PackageName) && !string.Equals(node.PackageName, packageInfoDic
                            [packageName].PackageName))
                        {
                            packageInfoDic[packageName].PackageDependencies.Add(node.PackageName);
                        }
                    }
                }
            }
            string[] assetNamesArray = assetNames.ToArray();
            assetBundleBuilds[buildIndex].assetBundleName = ComputeAssetSetSignature(assetNamesArray);
            assetBundleBuilds[buildIndex].assetNames = assetNamesArray;
            
            foreach (AssetBundleNode node in assetBundleNodeDic[key])
            {
                //��Ϊ�����˵�DerivedPackage
                //���Դ˴�����ȷ����ÿһ��Node������һ������
                AssetBuildInfo assetBuildInfo = new AssetBuildInfo();

                assetBuildInfo.AssetName = node.AssetName;
                assetBuildInfo.AssetBundleName = assetBundleBuilds[buildIndex].assetBundleName;

                packageInfoDic[node.PackageName].AssetInfos.Add(assetBuildInfo);
                
            }
            buildIndex++;
        }


        BuildPipeline.BuildAssetBundles(AssetBundleOutputPath, assetBundleBuilds, CheckIncrementalBuildMode(),
            BuildTarget.StandaloneWindows);

        string buildVersionFilePath = Path.Combine(BuildOutputPath, "BuildVersion.version");
        File.WriteAllText(buildVersionFilePath, AssetManagerConfig.CurrentBuildVersion.ToString());

        //�����汾·��
        string versionPath = Path.Combine(BuildOutputPath, AssetManagerConfig.CurrentBuildVersion.ToString());

        if (!Directory.Exists(versionPath))
        {
            //��·�������ھʹ���·��
            Directory.CreateDirectory(versionPath);
        }

        BuildAssetBundleHashTable(assetBundleBuilds,versionPath);

        CopyAssetBundleToVersionFolder(versionPath);

        BuildPackageTable(packageInfoDic, versionPath);

        AssetManagerConfig.CurrentBuildVersion++;
        //ˢ��Project���棬������Ǵ��������������Ҫִ��
        AssetDatabase.Refresh();

    }

    public static string PackageTableName = "AllPackages";
    /// <summary>
    /// 
    /// </summary>
    /// <param name="packages">Package�ֵ䣬keyΪ����</param>
    /// <param name="outputPath"></param>
    static void BuildPackageTable(Dictionary<string,PackageBuildInfo> packages,string versionPath)
    {
        
        string packagesPath = Path.Combine(AssetBundleOutputPath, PackageTableName);
        string packageVersionPath = Path.Combine(versionPath, PackageTableName);

        string packagesJSON = JsonConvert.SerializeObject(packages.Keys);

        File.WriteAllText(packagesPath, packagesJSON);
        File.WriteAllText(packageVersionPath, packagesJSON);

        foreach (PackageBuildInfo package in packages.Values)
        {
            packagesPath = Path.Combine(AssetBundleOutputPath, package.PackageName);
            packagesJSON = JsonConvert.SerializeObject(package);
            packageVersionPath = Path.Combine(versionPath, package.PackageName);

            File.WriteAllText(packagesPath, packagesJSON);
            File.WriteAllText(packageVersionPath, packagesJSON);
        }

    }

    static void CopyAssetBundleToVersionFolder(string versionPath)
    {
        //��AssetBundle���·���¶�ȡ���б�
        string[] assetNames = ReadAssetBundleHashTable(AssetBundleOutputPath);

        //��������
        string mainBundleOriginPath = Path.Combine(AssetBundleOutputPath, OutputBundleName);
        string mainBundleVersionPath = Path.Combine(versionPath, OutputBundleName);
        File.Copy(mainBundleOriginPath, mainBundleVersionPath,true);

        foreach (var assetName in assetNames)
        {
            string assetHashName = assetName.Substring(assetName.IndexOf("_") + 1);

            string assetOriginPath = Path.Combine(AssetBundleOutputPath, assetHashName);
            //fileInfo.Name�ǰ�������չ�����ļ���
            string assetVersionPath = Path.Combine(versionPath, assetHashName);
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

            //�����Լ�������Դ�����ã�ͬ��Ҳͨ������ͼ���д���
            if (!string.IsNullOrEmpty(lastNode.PackageName))
            {
                if (!currentNode.PackageNames.Contains(lastNode.PackageName))
                {
                    currentNode.PackageNames.Add(lastNode.PackageName);
                }

            }
            //������DerivedAsset,ֱ�ӻ�ȡlast Node��SourceIndices����
            else
            {
                foreach (string packageNames in lastNode.PackageNames)
                {
                    if (!currentNode.PackageNames.Contains(packageNames))
                    {
                        currentNode.PackageNames.Add(packageNames);
                    }
                }
            }

            //���lastNode��SourceAsset,��ֱ��Ϊ��ǰNode���last Node��Index
            //��ΪList��һ���������ͣ�����SourceAsset��Sourceindeies�������ݺ�derivedһ����Ҳ��Ϊһ���µ�List
            if (lastNode.SourceIndex >= 0)
            {
                if(!currentNode.SourceIndeices.Contains(lastNode.SourceIndex))
                {
                    currentNode.SourceIndeices.Add(lastNode.SourceIndex);
                }
                
            }
            //������DerivedAsset,ֱ�ӻ�ȡlast Node��SourceIndices����
            else
            {
                foreach (int index in lastNode.SourceIndeices)
                {
                    if (!currentNode.SourceIndeices.Contains(index))
                    {
                        currentNode.SourceIndeices.Add(index);
                    }
                }
            }
            GetNodeFromDependencies(currentNode, allNodes);
        }
    }


    public static List<string> GetSeletedAssetsDependencies()
    {
        List<string> depensencies = new List<string>();
        List<string> selecedAssets = new List<string>();
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

    public static string OutputBundleName = "LocalAssets";
    static void CheckBuildOutputPath()
    {
        
        switch (AssetManagerConfig.BuildingPattern)
        {
            case AssetBundlePattern.EditorSimulation:
                break;
            case AssetBundlePattern.Local:
                BuildOutputPath = Path.Combine(Application.streamingAssetsPath, "BuildOutput");
                break;
            case AssetBundlePattern.Remote:
                BuildOutputPath = Path.Combine(Application.persistentDataPath, "BuildOutput");
                break;
        }
        
        if (!Directory.Exists(BuildOutputPath))
        {
            //��·�������ھʹ���·��
            Directory.CreateDirectory(BuildOutputPath);
        }

        AssetBundleOutputPath = Path.Combine(BuildOutputPath, OutputBundleName);

        if (!Directory.Exists(AssetBundleOutputPath))
        {
            //��·�������ھʹ���·��
            Directory.CreateDirectory(AssetBundleOutputPath);
        }
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
        
        //��ѡ�н�Ҫ�������Դ�б�,���б�A
        List<string> selectedAssets = new List<string>();

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
        
        //��ѡ�н�Ҫ�������Դ�б�
        List<string> selectedAssets = new List<string>();

        //ѡ�ж��ٸ���Դ�������ٸ�AB��
        AssetBundleBuild[] assetBundleBuilds = new AssetBundleBuild[selectedAssets.Count];

        //string directoryPath = AssetDatabase.GetAssetPath(AssetManagerConfig.AssetBundleDirectory);

        for (int i = 0; i < assetBundleBuilds.Length; i++)
        {
            //string bundleName = selectedAssets[i].Replace($@"{directoryPath}\", string.Empty);

            //Unity������.prefab����ʱ����Ĭ��ʹ��Ԥ���嵼�������룬��assetBundle����Ԥ���壬���Իᵼ�±���
            //bundleName = bundleName.Replace(".prefab", string.Empty);

            //assetBundleBuilds[i].assetBundleName = bundleName;

            assetBundleBuilds[i].assetNames = new string[] { selectedAssets[i] };
        }

        BuildPipeline.BuildAssetBundles(AssetBundleOutputPath, assetBundleBuilds, CheckCompressionPattern(),
            BuildTarget.StandaloneWindows);

        //��ӡ���·��
        Debug.Log(AssetBundleOutputPath);

        //ˢ��Project���棬������Ǵ��������������Ҫִ��
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// ���ָ���ļ�����������ԴΪAssetBundle
    /// </summary>
    public static void BuildAssetBundleFromDirectory()
    {
        CheckBuildOutputPath();
        


        AssetBundleBuild[] assetBundleBuild = new AssetBundleBuild[1];

        //��Ҫ����ľ��������������������
        assetBundleBuild[0].assetBundleName = "Local";

        //������Ȼ��ΪName��ʵ������Ҫ��Դ�ڹ����µ�·��
        //assetBundleBuild[0].assetNames = AssetManagerConfig.CurrentAllAssets.ToArray();

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
        foreach (FileInfo info in fileInfos)
        {
            //.meta�ļ����������ļ�
            if (!isValidExtensionName(info.Extension))
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

}