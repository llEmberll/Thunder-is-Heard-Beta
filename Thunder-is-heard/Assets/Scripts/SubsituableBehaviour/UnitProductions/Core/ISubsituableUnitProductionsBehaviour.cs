using System.Collections.Generic;

public interface ISubsituableUnitProductionsBehaviour
{
    public void Init();

    public List<UnitProductionItem> GetItems(UnitProductions conductor);

    public bool IsAvailableToBuy(UnitProductionItem item);
    public void OnBuy(UnitProductionItem item);

}
