using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Contracts : ItemList
{
    public List<ContractItem> items;

    public string _contractType;
    public string _sourceObjectId;

    public override void Start()
    {
        InitContent();
    }

    public void Init(string contractType, string sourceObjectId)
    {
        _contractType = contractType; _sourceObjectId = sourceObjectId;

        FillContent();
    }

    public void InitContent()
    {
        content = GameObject.FindGameObjectWithTag(Tags.contractItems).transform;
    }

    public override void FillContent()
    {
        ClearItems();
        items = new List<ContractItem>();

        ContractCacheTable contractTable = Cache.LoadByType<ContractCacheTable>();
        foreach (var keyValuePair in contractTable.Items)
        {
            ContractCacheItem contractData = new ContractCacheItem(keyValuePair.Value.Fields);
            if (contractData.GetType() != _contractType)
            {
                continue;
            }

            items.Add(CreateContractItem(contractData));
        }
    }

    public ContractItem CreateContractItem(ContractCacheItem contractCacheData)
    {
        string id = contractCacheData.GetExternalId();
        string name = contractCacheData.GetName();
        ResourcesData cost = contractCacheData.GetCost();
        ResourcesData gives = contractCacheData.GetGives();
        int duration = contractCacheData.GetDuration();
        string description = contractCacheData.GetDescription();

        Sprite icon = ResourcesUtils.LoadIcon(contractCacheData.GetIconSection(), contractCacheData.GetIconName());

        GameObject itemObject = CreateObject(Config.resources["UI" + "ContractItemPrefab"], content);
        itemObject.name = name;
        ContractItem contractComponent = itemObject.GetComponent<ContractItem>();

        contractComponent.Init(
            id, 
            name, 
            _contractType, 
            _sourceObjectId,
            duration, 
            cost, 
            gives, 
            description,
            icon
            );
        return contractComponent;
    }
}
