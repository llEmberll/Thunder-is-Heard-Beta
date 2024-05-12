using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContractItem : Item
{
    public static string type = "Contract";
    public override string Type { get { return type; } }

    public string _contractType;
    public string _sourceObjectId;

    public ResourcesProcessor resourcesProcessor;

    public string coreId;

    public int _duration;
    public TMP_Text TmpDuration;

    public ResourcesData costData;
    public Transform cost;

    public ResourcesData givesData;
    public Transform gives;

    public void Init(
        string contractId, 
        string contractName, 
        string contractType,
        string sourceObjectId,
        int contractDuration, 
        ResourcesData contractCost, 
        ResourcesData contractGives, 
        string contractDescription = "", 
        Sprite contractIcon = null
        )
    {
        id = contractId; objName = contractName; icon = contractIcon; itemImage.sprite = icon;
        _contractType = contractType; _sourceObjectId = sourceObjectId;

        costData = contractCost;
        givesData = contractGives;
        description = contractDescription;
        _duration = contractDuration;

        UpdateUI();
    }

    public override void Awake()
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag("ResourcesProcessor").GetComponent<ResourcesProcessor>();
    }

    public override void UpdateUI()
    {
        TmpDuration.text = count.ToString();
        TmpDescription.text = description;

        ResourcesProcessor.UpdateResources(cost, costData);
        ResourcesProcessor.UpdateResources(gives, givesData);

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
            "Contract",
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
