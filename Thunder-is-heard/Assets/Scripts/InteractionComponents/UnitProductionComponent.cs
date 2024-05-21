using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UnitProductionComponent : InteractionComponent
{
    public UnitProductions _unitProductionsUI;

    public override void Init(string objectOnBaseId, string componentType)
    {
        base.Init(objectOnBaseId, componentType);

        _unitProductionsUI = Resources.FindObjectsOfTypeAll(typeof(UnitProductions)).First().GetComponent<UnitProductions>();
    }

    public override void Finished()
    {
        //��������� ����� �� unitId � ��������� � ��������� ������ � ��������� idle
        //������� ���� � ���� �� ������ ������������ ��������
        throw new System.NotImplementedException();
    }

    public override void HandleFinishedProcess(ProcessOnBaseCacheItem processCacheItem)
    {
        //������� ���� � ���� ��� �������� ������ �� ������ ������������ ��������
        throw new System.NotImplementedException();
    }

    public override void Idle()
    {
        ToggleUI();
        _unitProductionsUI.Init(type, id);
    }

    public override void Working()
    {
        //�������� ���������� ����� ����������
        Debug.Log("working...");
    }

    public override void HideUI()
    {
        _unitProductionsUI.Hide();
    }

    public override void ToggleUI()
    {
        _unitProductionsUI.Toggle();
    }
}
