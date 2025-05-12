using System;
using System.Collections.Generic;
using System.Text;

public class OnlyTutorialProductionsUnitProductionsBehaviour : BaseSubsituableUnitProductionsBehaviour
{

    public override void FillContent(UnitProductions conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<UnitProductionItem>();

        List<UnitProductionCacheItem> unitProductions = GenerateTutorialUnitProductions();
        foreach (UnitProductionCacheItem unitProductionData in unitProductions)
        {
            if (!IsUnitTypeMatch(unitProductionData.GetType(), conductor._unitProductionType))
            {
                continue;
            }

            conductor.items.Add(conductor.CreateUnitProductionItem(unitProductionData));
        }
    }

    private bool IsUnitTypeMatch(string unitType, string targetType)
    {
        return string.Equals(unitType, targetType, StringComparison.OrdinalIgnoreCase);
    }

    public List<UnitProductionCacheItem> GenerateTutorialUnitProductions()
    {
        UnitProductionCacheItem trainAssaultersUnitProduction = new UnitProductionCacheItem(new Dictionary<string, object>());
        trainAssaultersUnitProduction.SetExternalId("f4b60bee-1dda-4377-9fab-5092f48b3e60");
        trainAssaultersUnitProduction.SetName("Обучить новобранцев");
        trainAssaultersUnitProduction.SetDuration(3);
        ResourcesData cost = new ResourcesData(rubCount: 300);
        trainAssaultersUnitProduction.SetCost(cost);
        trainAssaultersUnitProduction.SetType(UnitTypes.infantry);
        trainAssaultersUnitProduction.SetUnitId("bd1b7986-cf1a-4d76-8b14-c68bf10f363f");
        trainAssaultersUnitProduction.SetIconSection("Textures/Interface/Cards/Units/Base");
        trainAssaultersUnitProduction.SetIconName("assaulters");
        return new List<UnitProductionCacheItem> { trainAssaultersUnitProduction };
    }
}
