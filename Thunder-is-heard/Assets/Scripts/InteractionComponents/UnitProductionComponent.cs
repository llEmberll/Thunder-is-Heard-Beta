using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UnitProductionComponent : InteractionComponent
{
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
        UnitProductions unitProductionsUI = Resources.FindObjectsOfTypeAll(typeof(UnitProductions)).First().GetComponent<UnitProductions>();
        unitProductionsUI.Toggle();
        unitProductionsUI.Init(type, id);
    }

    public override void Working()
    {
        //�������� ���������� ����� ����������
        Debug.Log("working...");
    }
}
