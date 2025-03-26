using UnityEngine;
using UnityEditor;
using MyPooler;

[CustomEditor(typeof(ObjectPooler))]
public class ObjectPoolerEditor : Editor
{
    Texture2D logoTex;
    Texture2D backGroundTex;

    public SerializedProperty pools, isDebug, shouldDestroyOnLoad;

    void OnEnable()
    {
        logoTex = (Texture2D)EditorGUIUtility.Load("Assets/MyPooler/Scripts/Editor/Images/PoolerHeader.png");
        backGroundTex = (Texture2D)EditorGUIUtility.Load("Assets/MyPooler/Scripts/Editor/Images/PoolerBg.png");

        isDebug = serializedObject.FindProperty("isDebug");
        shouldDestroyOnLoad = serializedObject.FindProperty("shouldDestroyOnLoad");
        pools = serializedObject.FindProperty("pools");

    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUI.color = Color.white;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
      
        DrawBackground();
        DrawLogo();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(isDebug, new GUIContent("IsDebug"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(shouldDestroyOnLoad, new GUIContent("Should Destroy on Load"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(pools, new GUIContent("Pools"));      
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawLogo()
    {
        Rect rect = GUILayoutUtility.GetLastRect();
        GUI.DrawTexture(new Rect(0, rect.yMin + 20, EditorGUIUtility.currentViewWidth, 130), logoTex, ScaleMode.ScaleToFit);
        GUILayout.Space(200);
    }
    private void DrawBackground()
    {
        Rect rect = GUILayoutUtility.GetLastRect();
        GUI.color = GUI.color = Color.white;
        GUI.DrawTexture(new Rect(0, rect.yMin, EditorGUIUtility.currentViewWidth, 550), backGroundTex);
        
    }
}
