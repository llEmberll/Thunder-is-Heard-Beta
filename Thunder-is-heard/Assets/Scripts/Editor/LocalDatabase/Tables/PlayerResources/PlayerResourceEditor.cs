using System.Data;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PlayerResourcesTable))]
public class PlayerResourceEditor : Editor
{
    public PlayerResourcesTable table;

    public void OnEnable()
    {
        table = (PlayerResourcesTable)target;
    }

    public override void OnInspectorGUI()
    {
        SetFirstElementAsCurrent();

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
    }

    public void SetFirstElementAsCurrent()
    {
        table.currentItem = table.items.Count == 0 ? new PlayerResourceData() : table.items[0];
    }
}