using UnityEditor;
using UnityEngine;

/// <summary>
/// 编辑器敌人生成工具
/// </summary>
public class SpawnTool : EditorWindow
{
    private GameObject enemyPrefab;
    private int spawnCount = 5;
    private float spawnRadius = 3f;

    [MenuItem("Tools/Enemy Spawner")]
    public static void ShowWindow()
    {
        GetWindow<SpawnTool>("Enemy Spawner");
    }

    private void OnGUI()
    {
        GUILayout.Label("敌人生成设置", EditorStyles.boldLabel);
        enemyPrefab = (GameObject)EditorGUILayout.ObjectField("敌人预制体", enemyPrefab, typeof(GameObject), false);
        spawnCount = EditorGUILayout.IntField("生成数量", spawnCount);
        spawnRadius = EditorGUILayout.FloatField("生成半径", spawnRadius);

        if (GUILayout.Button("批量生成"))
        {
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        if (!enemyPrefab) return;

        var center = SceneView.lastActiveSceneView.camera.transform.position;
        for (int i = 0; i < spawnCount; i++)
        {
            var randomPos = center + Random.insideUnitSphere * spawnRadius;
            var instance = PrefabUtility.InstantiatePrefab(enemyPrefab) as GameObject;
            instance.transform.position = randomPos;
        }
    }
}