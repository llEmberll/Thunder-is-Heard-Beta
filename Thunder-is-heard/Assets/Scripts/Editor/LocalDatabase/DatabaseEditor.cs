using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


[CustomEditor(typeof(LocalDatabase))]
public class DatabaseEditor : Editor
{
    private LocalDatabase database;

    public void OnEnable()
    {
        database = (LocalDatabase)target;
    }

    public override void OnInspectorGUI()
    {
            foreach (ScriptableObject table in database.GetTables())
            {
                Debug.Log("table name: " +  table.name);

                EditorGUILayout.BeginVertical("box");
                if (GUILayout.Button(table.name))
                {
                    Selection.activeObject = table;
            }

                EditorGUILayout.EndVertical();
            }

        base.OnInspectorGUI();
       
    }

    public static void SetObjectDirty(GameObject obj)
    {
        EditorUtility.SetDirty(obj);
        EditorSceneManager.MarkSceneDirty(obj.scene);
    }
}
