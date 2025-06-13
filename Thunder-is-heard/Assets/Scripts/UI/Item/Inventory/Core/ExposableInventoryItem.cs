using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public abstract class ExposableInventoryItem : InventoryItem
{
    public ResourcesData gives;

    public int health, damage, distance;

    public TMP_Text TmpHealth, TmpDamage, TmpDistance;

    public virtual void Init(string objectId, string objectName, ResourcesData objectGives, int objectHealth, int objectDamage, int objectDistance, int objectCount, string objectDescription = "", Sprite objectIcon = null)
    {
        _id = objectId; _objName = objectName; _icon = objectIcon; _itemImage.sprite = _icon;
        InitCoreId();

        gives = objectGives;
        _description = objectDescription;
        health = objectHealth; damage = objectDamage; distance = objectDistance; _count = objectCount;

        UpdateUI();
    }

    public override void UpdateUI()
    {
        TmpHealth.text = health.ToString();
        TmpDamage.text = damage.ToString();
        TmpDistance.text = distance.ToString();

        if (damage < 1)
        {
            TmpDamage.transform.parent.gameObject.SetActive(false);
        }

        if (distance < 1)
        {
            TmpDistance.transform.parent.gameObject.SetActive(false);
        }

        base.UpdateUI();
    }

    public void CreatePreview()
    {
        conductor.CreatePreview(this);
    }

    public virtual Bector2Int GetSize(CacheItem item)
    {
        if (Type == "Unit")
        {
            return new Bector2Int(new Vector2Int(1, 1));
        }

        object value = item.GetField("size");

        if (value is Bector2Int typedValue)
        {
            return typedValue;
        }

        return JsonConvert.DeserializeObject<Bector2Int>(value.ToString());
    }

    public void OnCancelExposing()
    {
        UnsubscribeAll();
    }

    public void Finish()
    {
        UnsubscribeAll();

        EventMaster.current.OnExitBuildMode();
    }

    public void UnsubscribeAll()
    {
        EventMaster.current.ToggledOffBuildMode -= OnCancelExposing;
        EventMaster.current.ObjectExposed -= OnObjectExposed;
    }

    public void Continue()
    {
    }

    public void OnObjectExposed(Entity obj)
    {
        conductor.OnObjectExposed(this, obj);
    }
}

