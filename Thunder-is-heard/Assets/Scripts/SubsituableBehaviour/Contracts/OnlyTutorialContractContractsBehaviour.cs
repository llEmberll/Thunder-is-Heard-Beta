using System.Collections.Generic;

public class OnlyTutorialContractContractsBehaviour : BaseSubsituableContractsBehaviour
{

    public override void FillContent(Contracts conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<ContractItem>();

        List<ContractCacheItem> tutorialContracts = GenerateContractItems();

        foreach (ContractCacheItem contractData in tutorialContracts)
        {
            if (contractData.GetType() != conductor._contractType)
            {
                continue;
            }

            conductor.items.Add(conductor.CreateContractItem(contractData));
        }
    }

    public List<ContractCacheItem> GenerateContractItems()
    {
        ContractCacheItem tutorialOilContractData = new ContractCacheItem(new Dictionary<string, object>());
        tutorialOilContractData.SetExternalId("453aa61c-ed38-449d-84bb-e79f96108bad");
        tutorialOilContractData.SetName("Канистра с топливом");
        tutorialOilContractData.SetDuration(3);
        ResourcesData tutorialOilContractGives = new ResourcesData();
        tutorialOilContractGives.oil = 5;
        tutorialOilContractData.SetGives(tutorialOilContractGives);

        ResourcesData tutorialOilContractCost = new ResourcesData();
        tutorialOilContractCost.rub = 250;
        tutorialOilContractData.SetCost(tutorialOilContractCost);

        tutorialOilContractData.SetIconSection("UIBuildCards");
        tutorialOilContractData.SetIconName("oil_station");


        ContractCacheItem tutorialRubContractData = new ContractCacheItem(new Dictionary<string, object>());
        tutorialRubContractData.SetExternalId("897d1863-f964-4809-8fc5-62f8ab4ecd9d");
        tutorialRubContractData.SetName("Бумажная волокита");
        tutorialRubContractData.SetDuration(3);
        ResourcesData tutorialRubContractGives = new ResourcesData();
        tutorialRubContractGives.rub = 1000;
        tutorialRubContractData.SetGives(tutorialRubContractGives);

        ResourcesData tutorialRubContractCost = new ResourcesData();
        tutorialRubContractCost.oil = 1;
        tutorialRubContractData.SetCost(tutorialRubContractCost);

        tutorialRubContractData.SetIconSection("UIBuildCards");
        tutorialRubContractData.SetIconName("office");

        return new List<ContractCacheItem> { tutorialRubContractData , tutorialOilContractData };
    }
}
