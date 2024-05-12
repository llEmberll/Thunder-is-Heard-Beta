using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Google.Protobuf.WellKnownTypes;

public class UnitProductionItem: Item
{
    public static string type = "UnitProduction";

    public override string Type { get { return type; } }

    public string _unitProductionType;
    public string _sourceObjectId;

    public ResourcesData costData;
    public string _unitId;
    public int health, damage, distance, mobility;

    public TMP_Text TmpHealth, TmpDamage, TmpDistance, TmpMobility, TmpDuration;

    public int _duration;

    public ResourcesProcessor resourcesProcessor;

    public override void Awake()
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag("ResourcesProcessor").GetComponent<ResourcesProcessor>();
    }

    public void Init(
        string productionId,
        string productionName,
        string productionType,
        string sourceObjectId,
        int productionDuration,
        ResourcesData productionCost,
        string unitId, 
        int unitHealth, 
        int unitDamage, 
        int unitDistance, 
        int unitMobility,
        string productionDescription = "", 
        Sprite productionIcon = null
        )
    {
        id = productionId; objName = productionName; icon = productionIcon; itemImage.sprite = icon;

        _unitProductionType = productionType; _sourceObjectId = sourceObjectId;
        _unitId = unitId;
        costData = productionCost;
        description = productionDescription;
        health = unitHealth; damage = unitDamage; distance = unitDistance; mobility = unitMobility; _duration = productionDuration;

        UpdateUI();
    }

    public override void UpdateUI()
    {
        TmpHealth.text = health.ToString();
        TmpDamage.text = damage.ToString();
        TmpDistance.text = distance.ToString();
        TmpMobility.text = mobility.ToString();

        TmpDuration.text = TimeUtils.GetTimeAsStringByMinutes(_duration);

        base.UpdateUI();
    }

    public override void Interact()
    {
        if (!IsAvailableToBuy()) return;

        OnBuy();
    }

    public bool IsAvailableToBuy()
    {
        return resourcesProcessor.IsAvailableToBuy(costData);
    }

    public void OnBuy()
    {
        float startTime = Time.realtimeSinceStartup;
        float endTime = startTime + (_duration * 60);

        ProcessWorker.CreateProcess(
            "UnitProduction",
            ProcessTypes.component,
            _sourceObjectId,
            startTime,
            endTime
            );
        resourcesProcessor.SubstractResources(costData);
        resourcesProcessor.Save();

        this.Toggle();
    }
}
