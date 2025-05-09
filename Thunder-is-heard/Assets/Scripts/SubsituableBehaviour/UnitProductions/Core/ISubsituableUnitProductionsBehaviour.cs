using System.Collections.Generic;

public interface ISubsituableUnitProductionsBehaviour
{
    public void Init(UnitProductions conductor);

    public List<UnitProductionItem> GetItems(UnitProductions conductor);

    public bool IsAvailableToBuy(UnitProductionItem item);
    public void OnBuy(UnitProductionItem item);

    public void Toggle(UnitProductions conductor);

    public void FillContent(UnitProductions conductor);

    public void OnInteractWithIdleComponent(UnitProductionComponent component);
    public void OnInteractWithWorkingComponent(UnitProductionComponent component);
    public void OnInteractWithFinishedComponent(UnitProductionComponent component);
}
