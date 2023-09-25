using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(PlayerUnitsTable))]
public class PlayerUnitEditor : Editor
{
    public PlayerUnitsTable table;
    public  void OnEnable()
    {
        table = (PlayerUnitsTable)target;
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
    }
}

