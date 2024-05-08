using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractComponent : InteractionComponent
{
    public override void Finished()
    {
        //���� �� ������ ������� �����, �� ��������� gives � ��������� ������ � ��������� idle
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
        Contracts contractsUI = GameObject.FindGameObjectWithTag(Tags.contracts).GetComponent<Contracts>();
        contractsUI.Toggle();
        contractsUI.Init(type, id);
    }

    public override void Working()
    {
        //�������� ���������� ����� ����������
        Debug.Log("working...");
    }
}
