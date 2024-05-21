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
        //Поместить юнита по unitId в инвентарь и перевести здание в состояние idle
        //Удалить сбор в кеше по данным завершенного процесса
        throw new System.NotImplementedException();
    }

    public override void HandleFinishedProcess(ProcessOnBaseCacheItem processCacheItem)
    {
        //Создать сбор в кеше для целевого здания по данным завершенного процесса
        throw new System.NotImplementedException();
    }

    public override void Idle()
    {
        ToggleUI();
        _unitProductionsUI.Init(type, id);
    }

    public override void Working()
    {
        //Показать оставшееся время выполнения
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
