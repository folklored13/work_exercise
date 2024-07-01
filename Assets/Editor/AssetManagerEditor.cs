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
        //Unity��Inspector������õ�AssetBundle��Ϣ����ʵ����һ��AssetBundle�ṹ��
        //Unityֻ�����Ǳ��������е��ļ��У������������ļ��������õ�AssetBundle�ռ����������
        BuildPipeline.BuildAssetBundles(AssetBundleOutputPath, assetBundleBuild, CheckCompressionPattern(),
            BuildTarget.StandaloneWindows);

        //��ӡ���·��
        Debug.Log(AssetBundleOutputPath);

        //ˢ��Project���棬������Ǵ��������������Ҫִ��
        AssetDatabase.Refresh();
    }

    public static List<string> FindAllAssetFromDirectory(string directoryPath)
    {
        List<string> assetPaths = new List<string>();
        //��������·��Ϊ�ջ��߲����ڵĻ�
        if(string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
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
        foreach(FileInfo info in fileInfos)
        {
            //.meta�ļ����������ļ�
            if(info.Extension.Contains(".meta"))
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