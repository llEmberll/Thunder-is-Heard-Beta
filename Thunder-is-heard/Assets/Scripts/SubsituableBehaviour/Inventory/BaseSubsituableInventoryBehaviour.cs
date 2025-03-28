using System.Collections.Generic;

public class BaseSubsituableInventoryBehaviour : ISubsituableInventoryBehaviour
{
    public virtual List<InventoryItem> GetItems(Inventory conductor)
    {
        return conductor.items;
    }

    public virtual void Init()
    {
    }

    public virtual void OnUse(InventoryItem item)
    {
        if (item is ExposableInventoryItem) OnUseExposableItem(item as ExposableInventoryItem);
        else
        {
            // No interact
        }
    }

    public virtual void OnUseExposableItem(ExposableInventoryItem item)
    {
        item.CreatePreview();

        EventMaster.current.OnBuildMode();
        EventMaster.current.ToggledOffBuildMode += item.OnCancelExposing;
        EventMaster.current.ObjectExposed += item.OnObjectExposed;
    }
}
