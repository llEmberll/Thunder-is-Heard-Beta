using UnityEngine.EventSystems;

public interface IItemConductor
{
    public void OnUse(InventoryItem item);

    public void CreatePreview(ExposableInventoryItem item);

    public void OnObjectExposed(ExposableInventoryItem item, Entity obj);

    public void Substract(InventoryItem item, int number = 1);

    public void OnPointerEnter(InventoryItem item, PointerEventData eventData);
    public void OnPointerExit(InventoryItem item, PointerEventData eventData);
} 