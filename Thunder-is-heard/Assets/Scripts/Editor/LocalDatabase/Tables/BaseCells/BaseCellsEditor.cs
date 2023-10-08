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

            if (GUILayout.Button("�������� �������"))
            {
                table.ClearAll();
            }

            if (GUILayout.Button("� ������ ������"))
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

            if (GUILayout.Button("������� �������"))
            {
                table.RemoveElement();
            }

            if (GUILayout.Button("����� �������"))
            {
                table.AddElement();
            }

        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();

        this.defaultCellsSize = EditorGUILayout.IntField("������:", this.defaultCellsSize);

        if (GUILayout.Button("������������� ������"))
        {
            table.GenerateDefaultCells(this.defaultCellsSize);
        }

        GUILayout.EndHorizontal();
    }
}