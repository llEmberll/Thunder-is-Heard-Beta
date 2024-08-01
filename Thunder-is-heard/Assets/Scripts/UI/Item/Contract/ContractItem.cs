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
        _id = contractId; _objName = contractName; 
        _icon = contractIcon; _itemImage.sprite = _icon;
        _contractType = contractType; _sourceObjectId = sourceObjectId;

        costData = contractCost;
        givesData = contractGives;
        _description = contractDescription;
        _duration = contractDuration;

        UpdateUI();
    }

    public override void Awake()
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag(Tags.resourcesProcessor).GetComponent<ResourcesProcessor>();
    }

    public override void UpdateUI()
    {
        TmpDescription.text = _description;

        ResourcesProcessor.UpdateResources(cost, costData);
        ResourcesProcessor.UpdateResources(gives, givesData);

        TmpDuration.text = TimeUtils.GetDHMTimeAsStringBySeconds(_duration);

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
        int startTime = (int)Time.realtimeSinceStartup;
        int endTime = startTime + (_duration);

        ProcessWorker.CreateProcess(
            Type,
            ProcessTypes.component,
            _sourceObjectId,
            startTime,
            endTime,
            new ProcessSource(Type, _id)
            );
        resourcesProcessor.SubstractResources(costData);
        resourcesProcessor.Save();
    }
}
