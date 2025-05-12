using System;
using System.Collections.Generic;
using System.Text;

public class OnlyTutorialContractContractsBehaviour : BaseSubsituableContractsBehaviour
{

    public override void FillContent(Contracts conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<ContractItem>();

        List<ContractCacheItem> contracts = GenerateTutorialContracts();
        foreach (ContractCacheItem contractData in contracts)
        {
            if (!IsContractTypeMatch(contractData.GetType(), conductor._contractType))
            {
                continue;
            }

            conductor.items.Add(conductor.CreateContractItem(contractData));
        }
    }

    private bool IsContractTypeMatch(string contractType, string targetType)
    {
        return string.Equals(contractType, targetType, StringComparison.OrdinalIgnoreCase);
    }

    public List<ContractCacheItem> GenerateTutorialContracts()
    {
        ContractCacheItem tutorialOilContractData = new ContractCacheItem(new Dictionary<string, object>());
        tutorialOilContractData.SetExternalId("453aa61c-ed38-449d-84bb-e79f96108bad");
        tutorialOilContractData.SetName("Канистра с топливом");
        tutorialOilContractData.SetDuration(3);
        ResourcesData tutorialOilContractGives = new ResourcesData(oilCount: 5);
        tutorialOilContractData.SetGives(tutorialOilContractGives);

        ResourcesData tutorialOilContractCost = new ResourcesData(rubCount: 250);
        tutorialOilContractData.SetCost(tutorialOilContractCost);

        tutorialOilContractData.SetIconSection("UIBuildCards");
        tutorialOilContractData.SetIconName("oil_station");


        ContractCacheItem tutorialRubContractData = new ContractCacheItem(new Dictionary<string, object>());
        tutorialRubContractData.SetExternalId("897d1863-f964-4809-8fc5-62f8ab4ecd9d");
        tutorialRubContractData.SetName("Бумажная волокита");
        tutorialRubContractData.SetDuration(3);
        ResourcesData tutorialRubContractGives = new ResourcesData(rubCount: 1000);
        tutorialRubContractData.SetGives(tutorialRubContractGives);

        ResourcesData tutorialRubContractCost = new ResourcesData(oilCount: 1);
        tutorialRubContractData.SetCost(tutorialRubContractCost);

        tutorialRubContractData.SetIconSection("UIBuildCards");
        tutorialRubContractData.SetIconName("office");

        return new List<ContractCacheItem> { tutorialRubContractData , tutorialOilContractData };
    }
}
