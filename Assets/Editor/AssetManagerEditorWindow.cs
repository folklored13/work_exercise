using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//窗口渲染
public class AssetManagerEditorWindow : EditorWindow
{
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

        //加载图片资源到编辑器窗口中
        LogoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ootw.jpg");
        LogoTextureStyle = new GUIStyle();
        LogoTextureStyle.alignment = TextAnchor.MiddleCenter;
    }
    /// <summary>
    /// 这个方法会在每个渲染帧调用，所以可以用来渲染UI界面
    /// 因为该方法运行在Editor类中，所以刷新频率取决于Editor的运行帧率
    /// </summary>
    private void OnGUI()
    {
        //默认情况下垂直排版
        //GUI按照代码顺序进行渲染
        GUILayout.Space(20);

        if (LogoTexture != null)
        {
            GUILayout.Label(LogoTexture,LogoTextureStyle);
        }

        #region Title文字内容
        GUILayout.Space(20);
        GUILayout.Label(nameof(AssetManagerEditor),TitleTextStyle);

        #endregion
        GUILayout.Space(20);
        GUILayout.Label(AssetManagerEditor.AssetManagerVersion,VersionTextStyle);

        GUILayout.Space(20);
        AssetManagerEditor.BuildingPattern = (AssetBundlePattern)EditorGUILayout.EnumPopup("打包模式",
            AssetManagerEditor.BuildingPattern);

        GUILayout.Space(20);
        AssetManagerEditor.CompressionPattern = (AssetBundleCompressionPattern)EditorGUILayout.EnumPopup("压缩格式",
            AssetManagerEditor.CompressionPattern);

        GUILayout.Space(20);
        AssetManagerEditor.AssetBundleDirectory = EditorGUILayout.ObjectField(AssetManagerEditor.AssetBundleDirectory, typeof
            (DefaultAsset), true) as DefaultAsset;

        GUILayout.Space(20);
        if (GUILayout.Button("打包AssetBundle"))
        {
            AssetManagerEditor.BuildAssetBundleFromDirectory();
            Debug.Log("EditorButton按下");
        }
    }
}
