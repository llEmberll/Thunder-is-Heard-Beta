using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BaseCellsTable))]
public class BaseCellsEditor : Editor
{
    public BaseCellsTable table;

    public int defaultCellsSize = 35;


    public void OnEnable()
    {
        table = (BaseCellsTable)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();

            if (GUILayout.Button("очистить таблицу"))
            {
                table.ClearAll();
            }

            if (GUILayout.Button("к списку таблиц"))
            {
                Selection.activeObject = new LocalDatabase();
            }

        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();

            if (GUILayout.Button("<="))
            {
                table.GetPrev();
            }

            if (GUILayout.Button("=>"))
            {
                table.GetNext();
            }

        GUILayout.EndHorizontal();


        base.OnInspectorGUI();


        GUILayout.BeginHorizontal();

            if (GUILayout.Button("удалить элемент"))
            {
                table.RemoveElement();
            }

            if (GUILayout.Button("новый элемент"))
            {
                table.AddElement();
            }

        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();

        this.defaultCellsSize = EditorGUILayout.IntField("размер:", this.defaultCellsSize);

        if (GUILayout.Button("сгенерировать клетки"))
        {
            table.GenerateDefaultCells(this.defaultCellsSize);
        }

        GUILayout.EndHorizontal();
    }
}