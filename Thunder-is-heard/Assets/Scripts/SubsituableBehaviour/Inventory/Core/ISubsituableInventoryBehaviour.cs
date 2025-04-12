using System.Collections.Generic;

public interface ISubsituableInventoryBehaviour
{
    public void Init(Inventory conductor);

    public List<InventoryItem> GetItems(Inventory conductor);

    public void OnUse(InventoryItem item);

    public void Toggle(Inventory conductor);

    public void FillContent(Inventory conductor);
}
