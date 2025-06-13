using System.Collections.Generic;

public interface ISubsituableInventoryBehaviour
{
    public void Init(Inventory conductor);

    public List<InventoryItem> GetItems(Inventory conductor);

    public void OnUse(InventoryItem item);

    public void CreatePreview(Inventory conductor, ExposableInventoryItem item);

    public void OnObjectExposed(Inventory conductor, ExposableInventoryItem item, Entity obj);

    public void Substract(Inventory conductor, InventoryItem item, int number = 1);

    public void Toggle(Inventory conductor);

    public void FillContent(Inventory conductor);

    public BuildInventoryItem CreateBuild(Inventory conductor, InventoryCacheItem inventoryItemData, BuildCacheItem buildData);
    
    public UnitInventoryItem CreateUnit(Inventory conductor, InventoryCacheItem inventoryItemData, UnitCacheItem unitData);
    
    public MaterialInventoryItem CreateMaterial(Inventory conductor, InventoryCacheItem inventoryItemData, MaterialCacheItem materialData);
}
