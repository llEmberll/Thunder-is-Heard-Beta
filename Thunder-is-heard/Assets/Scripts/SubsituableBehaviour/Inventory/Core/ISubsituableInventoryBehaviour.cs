using System.Collections.Generic;

public interface ISubsituableInventoryBehaviour
{
    public void Init();

    public List<InventoryItem> GetItems(Inventory conductor);

    public void OnUse(InventoryItem item);

}
