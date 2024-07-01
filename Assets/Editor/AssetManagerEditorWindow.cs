using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//������Ⱦ
public class AssetManagerEditorWindow : EditorWindow
{
<<<<<<< HEAD
    public static GUIStyle TitleTextStyle;
    public static GUIStyle VersionTextStyle;
    public static Texture2D LogoTexture;
    public static GUIStyle LogoTextureStyle;

    public void Awake()
    {
        TitleTextStyle = new GUIStyle();
        TitleTextStyle.fontSize = 26;
        TitleTextStyle.normal.textColor = Color.red;
        TitleTextStyle.alignment = TextAnchor.MiddleCenter;

        VersionTextStyle = new GUIStyle();
        VersionTextStyle.fontSize = 20;
        VersionTextStyle.normal.textColor = Color.white;
        VersionTextStyle.alignment = TextAnchor.MiddleRight;

        //����ͼƬ��Դ���༭��������
        LogoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ootw.jpg");
        LogoTextureStyle = new GUIStyle();
        LogoTextureStyle.alignment = TextAnchor.MiddleCenter;
    }
=======

    public string VersionString;

    public AssetManagerEditorWindowConfigSO WindowConfig;

    public void Awake()
    {
        AssetManagerEditor.LoadConfig(this);
        AssetManagerEditor.LoadWindowConfig(this);
    }

    /// <summary>
    /// ÿ�����̷����޸�ʱ������ø÷���
    /// </summary>
    private void OnValidate()
    {
        AssetManagerEditor.LoadConfig(this);
        AssetManagerEditor.LoadWindowConfig(this);

    }

    private void OnInspectorUpdate()
    {
        AssetManagerEditor.LoadConfig(this);
        AssetManagerEditor.LoadWindowConfig(this);
    }

    private void OnEnable()
    {
        AssetManagerEditor.AssetManagerConfig.GetCurrentDirectoryAllAssets();
    }

    public DefaultAsset editorWindowDirectory = null;

>>>>>>> d91a4b8 (ScriptableObject的使用)
    /// <summary>
    /// �����������ÿ����Ⱦ֡���ã����Կ���������ȾUI����
    /// ��Ϊ�÷���������Editor���У�����ˢ��Ƶ��ȡ����Editor������֡��
    /// </summary>
    private void OnGUI()
    {
        //Ĭ������´�ֱ�Ű�
        //GUI���մ���˳�������Ⱦ
        GUILayout.Space(20);

<<<<<<< HEAD
        if (LogoTexture != null)
        {
            GUILayout.Label(LogoTexture,LogoTextureStyle);
=======
        if (WindowConfig.LogoTexture != null)
        {
            GUILayout.Label(WindowConfig.LogoTexture, WindowConfig.LogoTextureStyle);
>>>>>>> d91a4b8 (ScriptableObject的使用)
        }

        #region Title��������
        GUILayout.Space(20);
<<<<<<< HEAD
        GUILayout.Label(nameof(AssetManagerEditor),TitleTextStyle);

        #endregion
        GUILayout.Space(20);
        GUILayout.Label(AssetManagerEditor.AssetManagerVersion,VersionTextStyle);

        GUILayout.Space(20);
        AssetManagerEditor.BuildingPattern = (AssetBundlePattern)EditorGUILayout.EnumPopup("���ģʽ",
            AssetManagerEditor.BuildingPattern);

        GUILayout.Space(20);
        AssetManagerEditor.CompressionPattern = (AssetBundleCompressionPattern)EditorGUILayout.EnumPopup("ѹ����ʽ",
            AssetManagerEditor.CompressionPattern);

        GUILayout.Space(20);
        AssetManagerEditor.AssetBundleDirectory = EditorGUILayout.ObjectField(AssetManagerEditor.AssetBundleDirectory, typeof
            (DefaultAsset), true) as DefaultAsset;

        GUILayout.Space(20);
        if (GUILayout.Button("���AssetBundle"))
        {
            AssetManagerEditor.BuildAssetBundleFromDirectory();
            Debug.Log("EditorButton����");
        }
=======
        GUILayout.Label(nameof(AssetManagerEditor), WindowConfig.TitleTextStyle);

        #endregion
        GUILayout.Space(20);
        GUILayout.Label(VersionString, WindowConfig.VersionTextStyle);

        GUILayout.Space(20);
        AssetManagerEditor.AssetManagerConfig.BuildingPattern = (AssetBundlePattern)EditorGUILayout.EnumPopup("���ģʽ",
            AssetManagerEditor.AssetManagerConfig.BuildingPattern);

        GUILayout.Space(20);
        AssetManagerEditor.AssetManagerConfig.CompressionPattern = (AssetBundleCompressionPattern)EditorGUILayout.EnumPopup("ѹ����ʽ",
            AssetManagerEditor.AssetManagerConfig.CompressionPattern);

        GUILayout.Space(20);
        AssetManagerEditor.AssetManagerConfig._IncrementalBuildMode = (IncrementalBuildMode)EditorGUILayout.EnumPopup("�������",
            AssetManagerEditor.AssetManagerConfig._IncrementalBuildMode);

        GUILayout.Space(20);

        editorWindowDirectory = EditorGUILayout.ObjectField(AssetManagerEditor.AssetManagerConfig.AssetBundleDirectory, typeof
            (DefaultAsset), true) as DefaultAsset;

        if(AssetManagerEditor.AssetManagerConfig.AssetBundleDirectory!=editorWindowDirectory)
        {
            if(editorWindowDirectory==null)
            {
                AssetManagerEditor.AssetManagerConfig.CurrentAllAssets.Clear();
            }
            AssetManagerEditor.AssetManagerConfig.AssetBundleDirectory = editorWindowDirectory;
            AssetManagerEditor.AssetManagerConfig.GetCurrentDirectoryAllAssets();
        }

        if(AssetManagerEditor.AssetManagerConfig.CurrentAllAssets !=null && AssetManagerEditor.AssetManagerConfig.CurrentAllAssets.Count>0)
        {
            for(int i=0;i<AssetManagerEditor.AssetManagerConfig.CurrentAllAssets.Count;i++)
            {
                AssetManagerEditor.AssetManagerConfig.CurrentSelectedAssets[i] = EditorGUILayout.ToggleLeft(AssetManagerEditor.AssetManagerConfig.CurrentAllAssets[i], AssetManagerEditor.AssetManagerConfig.CurrentSelectedAssets[i]);
            }
        }

        GUILayout.Space(20);
        if (GUILayout.Button("���AssetBundle"))
        {
            AssetManagerEditor.BuildAssetBundleFromDirectedGraph();
            Debug.Log("EditorButton����");
        }

        GUILayout.Space(20);
        if (GUILayout.Button("����Config�ļ�"))
        {
            AssetManagerEditor.SaveConfigToJson();
        }

        GUILayout.Space(20);
        if (GUILayout.Button("��ȡConfigJson�ļ�"))
        {
            AssetManagerEditor.LoadConfigFromJson();
        }
>>>>>>> d91a4b8 (ScriptableObject的使用)
    }
}
