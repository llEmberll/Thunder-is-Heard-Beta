using UnityEngine;
using TMPro;

public class ContractItem : Item
{
    public static string type = "Contract";
    public override string Type { get { return type; } }

    public string _contractType;
    public string _sourceObjectId;

    public int _duration;
    public TMP_Text TmpDuration;

    public ResourcesData costData;
    public Transform cost;

    public ResourcesData givesData;
    public Transform gives;

    public Contracts conductor;

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

    public void SetConductor(Contracts value)
    {
        conductor = value;
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
        return conductor.IsAvailableToBuy(this);
    }

    public void OnBuy()
    {
        conductor.OnBuy(this);
    }
}
