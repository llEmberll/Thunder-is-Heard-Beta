using TMPro;
using UnityEngine.EventSystems;

public abstract class InventoryItem : Item
{
    public string coreId;

    public TMP_Text TmpCount;

    public IItemConductor conductor;

    public void SetConductor(IItemConductor value)
    {
        conductor = value;
    }

    public override void EnableListeners()
    {
        EventMaster.current.InventoryItemAdded += OnInventoryItemAdded;
    }

    public override void DisableListeners()
    {
        EventMaster.current.InventoryItemAdded -= OnInventoryItemAdded;
    }

    public virtual void OnInventoryItemAdded(InventoryCacheItem item)
    {
        if (this.Type.Contains(item.GetType()) && item.GetCoreId() == this.coreId) {
            Increment();
        }
    }

    public override void Interact()
    {
        OnUse();
    }

    public void OnUse()
    {
        conductor.OnUse(this);
    }

    public override void UpdateUI()
    {
        TmpCount.text = _count.ToString();
        TmpDescription.text = _description;

        base.UpdateUI();
    }

    public override void UpdateCount(int newCount)
    {
        base.UpdateCount(newCount);
        TmpCount.text = newCount.ToString();
    }

    public void Substract(int number = 1)
    {
        conductor.Substract(this, number);
    }

    public void InitCoreId(string coreId)
    {
        this.coreId = coreId;
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        conductor.OnPointerEnter(this, data);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        conductor.OnPointerExit(this, eventData);
    }
}

