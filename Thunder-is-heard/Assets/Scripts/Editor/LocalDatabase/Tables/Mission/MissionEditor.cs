using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MissionsTable))]
public class MissionEditor : Editor
{
    public MissionsTable table;

    public void OnEnable()
    {
        table = (MissionsTable)target;
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
