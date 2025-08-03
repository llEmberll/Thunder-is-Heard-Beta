using System.Collections.Generic;

public interface ISubsituableLandingBehaviour
{
    public void Init(Landing conductor);

    public void OnObjectRemoved(Landing conductor, Entity entity);
    public void OnObjectLanded(Landing conductor, Entity entity);

    public void StartLanding(Landing conductor, LandingData landingData);
    public void OnPressedToBattleButton(Landing conductor);
    public void FinishLanding(Landing conductor);

    public void OnLandableUnitFocus(Landing conductor, LandableUnit target);
    public void OnLandableUnitDefocus(Landing conductor, LandableUnit target);

    public List<LandableUnit> GetItems(Landing conductor);
    public void FillContent(Landing conductor);
    public LandableUnit CreateUnit(Landing conductor, InventoryCacheItem inventoryItemData, UnitCacheItem unitData);

    public void OnUse(Landing conductor, InventoryItem target);

    public void CreatePreview(Landing conductor, ExposableInventoryItem item);

    public void OnObjectExposed(Landing conductor, ExposableInventoryItem item, Entity obj);

    public void OnInventoryItemAdded(Landing conductor, InventoryItem sourceItem, InventoryCacheItem addedItem);

    public void Substract(Landing conductor, InventoryItem item, int number = 1);

    public void Increment(Landing conductor, InventoryItem item, int number = 1);
}
