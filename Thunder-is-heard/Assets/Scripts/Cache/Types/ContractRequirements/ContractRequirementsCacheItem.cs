using Newtonsoft.Json;
using System;
using System.Collections.Generic;


[System.Serializable]
public class ContractRequirementsCacheItem : CacheItem
{
    public ContractRequirementsCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("data"))
        {
            SetData(new ContractRequirementsData());
        }
    }

    public string GetContractId()
    {
        return (string)GetField("contractId");
    }

    public void SetContractId(string value)
    {
        SetField("contractId", value);
    }

    public ContractRequirementsData GetData()
    {
        object value = GetField("data");
        if (value == null)
        {
            return new ContractRequirementsData();
        }

        return JsonConvert.DeserializeObject<ContractRequirementsData>(value.ToString());
    }

    public void SetData(ContractRequirementsData value)
    {
        SetField("data", value);
    }


    public override CacheItem Clone()
    {
        ContractRequirementsCacheItem clone = new ContractRequirementsCacheItem(fields);
        return clone;
    }
}
