using System.Collections;
using System.Collections.Generic;
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
        id = objectId; objName = objectName; icon = objectIcon;
        InitCoreId();

        gives = objectGives;
        description = objectDescription;
        health = objectHealth; damage = objectDamage; distance = objectDistance; count = objectCount;

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

    public override void Interact()
    {
        CreatePreview();

        EventMaster.current.OnBuildMode();
        EventMaster.current.ToggledOffBuildMode += OnCancelExposing;
        EventMaster.current.ObjectExposed += OnObjectExposed;
    }

    public void CreatePreview()
    {
        CacheTable itemsTable = Cache.LoadByName(Type);

        CacheItem needleItemData = itemsTable.GetById(coreId);
        if (needleItemData == null)
        {
            Debug.Log("CreatePreview | Can't find item by id: " + coreId);
            Finish();
            return;
        }

        string modelPath = (string)needleItemData.GetField("modelPath");
        Vector2Int size = GetSize(needleItemData).ToVector2Int();
        GameObject modelPrefab = Resources.Load<GameObject>(modelPath);
        Transform model = Instantiate(modelPrefab).transform;

        ObjectPreview preview = ObjectPreview.Create();
        preview.Init(objName, Type, coreId, size, model);
    }

    public virtual Bector2Int GetSize(CacheItem item)
    {
        if (Type == "Unit")
        {
            return new Bector2Int(new Vector2Int(1, 1));
        }

        object value = item.GetField("size");
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

    public void OnObjectExposed(string objId, string objType, Bector2Int[] occypation, int rotation)
    {
        if (objId == coreId && objType == Type)
        {
            if (count < 2)
            {
                Finish();
            }

            Substract();
        }

        else
        {
            Continue();
        }
    }
}

