using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetManagerConfig", menuName = "AssetManager/CreateManagerConfig")]
public class AssetManagerConfigScriptableObject : ScriptableObject
{
    /// <summary>
    /// �༭��ģ���£������д��
    /// ����ģʽ�������StreamingAssets
    /// Զ��ģʽ�����������Զ��·�����ڸ�ʾ����ΪpersistentDataPath
    /// </summary>
    public AssetBundlePattern BuildingPattern;

    /// <summary>
    /// �Ƿ�Ӧ���������
    /// </summary>
    public IncrementalBuildMode _IncrementalBuildMode;

    /// <summary>
    /// AssetBundleѹ����ʽ
    /// </summary>
    public AssetBundleCompressionPattern CompressionPattern;

    /// <summary>
    /// ��Դ���������ߵİ汾
    /// </summary>
    public int AssetManagerVersion = 100;

    /// <summary>
    /// ��Դ����İ汾
    /// </summary>
    public int CurrentBuildVersion = 100;


    /// <summary>
    /// ��Ҫ������ļ���
    /// </summary>
    [SerializeField]
    public DefaultAsset AssetBundleDirectory;


    /// <summary>
    /// ���ļ��б�����ֵʱ�����ڴ�����ļ�����������Դ·��
    /// </summary>
    public List<string> CurrentAllAssets = new List<string>();

    /// <summary>
    /// ��Editor������ѡ�����Դ���������������Ӧ
    /// </summary>
    public bool[] CurrentSelectedAssets;

    public void GetCurrentDirectoryAllAssets()
    {
        if (AssetBundleDirectory != null)
        {
            return;
        }
        string directoryPath = AssetDatabase.GetAssetPath(AssetBundleDirectory);
        CurrentAllAssets = FindAllAssetFromDirectory(directoryPath);
        CurrentSelectedAssets = new bool[CurrentAllAssets.Count];

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
        }
        return assetPaths;
    }

    /// <summary>
    /// ��Ҫ�ų���Asset��չ��
    /// </summary>
    public string[] InvalidExtensionNames = new string[] { ".meta", ".cs" };

    public bool isValidExtensionName(string fileName)
    {
        bool isValid = true;
        foreach (string invalidName in InvalidExtensionNames)
        {
            if (fileName.Contains(invalidName))
            {
                isValid = false;
                return isValid;
            }
        }
        return isValid;
    }
}
