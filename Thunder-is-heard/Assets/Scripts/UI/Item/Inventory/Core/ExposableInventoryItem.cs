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

        CacheItem needleItemData = itemsTable.GetById(id);
        if (needleItemData == null)
        {
            Debug.Log("CreatePreview | Can't find item by id: " + id);
            Finish();
            return;
        }

        string modelPath = (string)needleItemData.GetField("modelPath");
        Bector2Int size = GetSize(needleItemData);

        Transform previewPrefab = Resources.Load(Config.resources["prefabPreview"], typeof(Transform)) as Transform;
        var previewObject = Instantiate(previewPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        ObjectPreview preview = previewObject.GetComponent<ObjectPreview>();
        preview.Init(objName, Type, id, size, modelPath);
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
        if (objId == id && objType == Type)
        {
            SaveExpose(occypation, rotation);

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

    public abstract void SaveExpose(Bector2Int[] occypation, int rotation);
}

