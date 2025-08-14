
using System;
using UnityEngine;

public class UnitProductionInProcessCondition: BasicCondition
{
    public bool firstCheck = true;

    public string _targetUnitProductionId;
    public bool process = false;

    // Добавляем уникальный идентификатор для логирования
    private string _logPrefix;

    public UnitProductionInProcessCondition(string targetUnitProductionId) 
    {
        _targetUnitProductionId = targetUnitProductionId;
        _logPrefix = $"[UnitProductionInProcessCondition_{_targetUnitProductionId}]";
        Debug.Log($"{_logPrefix} Конструктор вызван для UnitProductionId: {_targetUnitProductionId}");
        // Убираем EnableListeners() из конструктора - теперь это будет в OnActivate
    }

    public void FirstComplyCheck()
    {
        Debug.Log($"{_logPrefix} FirstComplyCheck() вызван");
        firstCheck = false;

        process = IsTargetUnitProductionInProcess();
        Debug.Log($"{_logPrefix} Результат проверки: process = {process}");

        if (process)
        {
            Debug.Log($"{_logPrefix} Производство уже в процессе, отключаем слушатели");
            DisableListeners();
        }
    }

    public bool IsTargetUnitProductionInProcess()
    {
        Debug.Log($"{_logPrefix} IsTargetUnitProductionInProcess() вызван");
        ProcessOnBaseCacheTable processTable = Cache.LoadByType<ProcessOnBaseCacheTable>();
        CacheItem processData = processTable.FindBySourceId(_targetUnitProductionId);
        bool result = processData != null;
        Debug.Log($"{_logPrefix} Поиск в таблице процессов: {result}, ID: {_targetUnitProductionId}");
        return result;
    }

    public void EnableListeners()
    {
        Debug.Log($"{_logPrefix} EnableListeners() - подписываемся на события");
        EventMaster.current.ProcessOnBaseStarted += SomeProcessOnBaseStarted;
    }

    public void DisableListeners()
    {
        Debug.Log($"{_logPrefix} DisableListeners() - отписываемся от событий");
        EventMaster.current.ProcessOnBaseStarted -= SomeProcessOnBaseStarted;
    }

    private bool CheckProcessSourceType(string processType)
    {
        bool result = string.Equals(processType, UnitProductionItem.type, StringComparison.OrdinalIgnoreCase);
        Debug.Log($"{_logPrefix} CheckProcessSourceType: {processType} == {UnitProductionItem.type} = {result}");
        return result;
    }

    public void SomeProcessOnBaseStarted(ProcessOnBaseCacheItem processData)
    {
        Debug.Log($"{_logPrefix} SomeProcessOnBaseStarted() вызван для процесса: {processData?.GetSource()?.id}");
        
        if (!CheckProcessSourceType(processData.GetSource().type)) 
        {
            Debug.Log($"{_logPrefix} Тип процесса не совпадает, игнорируем");
            return;
        }
        
        if (processData.GetSource().id != _targetUnitProductionId) 
        {
            Debug.Log($"{_logPrefix} ID процесса не совпадает, игнорируем");
            return;
        }

        Debug.Log($"{_logPrefix} Процесс соответствует условию, устанавливаем process = true");
        process = true;
        DisableListeners();
    }

    protected override void OnActivate()
    {
        Debug.Log($"{_logPrefix} OnActivate() вызван, firstCheck = {firstCheck}, process = {process}");
        
        // При активации проверяем текущее состояние
        if (firstCheck)
        {
            Debug.Log($"{_logPrefix} Первая проверка, вызываем FirstComplyCheck()");
            FirstComplyCheck();
        }
        if (!process)
        {
            Debug.Log($"{_logPrefix} Уже проверяли и производство не в процессе, подписываемся на события");
            // Если уже проверяли и производство не в процессе, подписываемся на события
            EnableListeners();
        }
    }
    
    protected override void OnDeactivate()
    {
        Debug.Log($"{_logPrefix} OnDeactivate() вызван");
        DisableListeners();
    }
    
    protected override void OnReset()
    {
        Debug.Log($"{_logPrefix} OnReset() вызван");
        firstCheck = true;
        process = false;
        DisableListeners();
    }

    public override bool IsComply()
    {
        if (firstCheck && _isActive)
        {
            FirstComplyCheck();
        }

        return process;
    }

    public override bool IsRealTimeUpdate()
    {
        return true;
    }
}
