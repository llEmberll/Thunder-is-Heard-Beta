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
            foreach (var table in LocalDatabase.GetTables())
            {
                Debug.Log("table name: " +  ((ITable)table).Name);

                EditorGUILayout.BeginVertical("box");
                if (GUILayout.Button(((ITable)table).Name))
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
