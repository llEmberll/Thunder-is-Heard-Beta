using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shop : ItemList
{
    public List<ShopItem> items;
    public Transform content;

    public override void Start()
    {
        EventMaster.current.ShopChanged += UpdateContent;

        InitContent();

        base.Start();
    }

    public void IncreaseItem(string id, string type, int count)
    {
        foreach (var item in items)
        {
            if (item.coreId == id && item.Type == type)
            {
                item.Increment(count);
                break;
            }
        }
    }

    public override void FillContent()
    {
        ClearItems();

        //Согласно рангу, проверить лимиты купленных объектов и сделать нужное количество

        ShopCacheTable shopTable = Cache.LoadByType<ShopCacheTable>();
        foreach (var keyValuePair in shopTable.Items)
        {
            ShopCacheItem shopItemData = new ShopCacheItem(keyValuePair.Value.Fields);
            string type = shopItemData.GetType();

            CacheTable itemTable = Cache.LoadByName(type);
            CacheItem item = itemTable.GetById(shopItemData.GetCoreId());

            switch (type)
            {
                case "Build":
                    BuildCacheItem buildData = new BuildCacheItem(item.Fields);
                    BuildShopItem build = CreateBuild(shopItemData, buildData);
                    items.Add(build);
                    break;
                case "Unit":
                    UnitCacheItem unitData = new UnitCacheItem(item.Fields);
                    UnitShopItem unit = CreateUnit(shopItemData, unitData);
                    items.Add(unit);
                    break;
                case "Material":
                    MaterialCacheItem materialData = new MaterialCacheItem(item.Fields);
                    MaterialShopItem material = CreateMaterial(shopItemData, materialData);
                    items.Add(material);
                    break;
            }
        }
    }

    public void UpdateContent()
    {
        FillContent();
    }

    public BuildShopItem CreateBuild(ShopCacheItem shopItemData, BuildCacheItem buildData)
    {
        string id = shopItemData.GetExternalId();
        string name = buildData.GetName();
        ResourcesData cost = buildData.GetCost();
        ResourcesData gives = buildData.GetGives();
        int health = buildData.GetHealth();
        int damage = buildData.GetDamage();
        int distance = buildData.GetDistance();
        int count = shopItemData.GetCount();
        string description = buildData.GetDescrption();
        Sprite[] iconSection = Resources.LoadAll<Sprite>(buildData.GetIconSection());
        Sprite icon = SpriteUtils.FindSpriteByName(buildData.GetIconName(), iconSection);

        GameObject itemObject = CreateObject(Config.resources["UI" + "Build" + "ShopItemPrefab"], content);
        itemObject.name = name;
        BuildShopItem buildComponent = itemObject.GetComponent<BuildShopItem>();

        buildComponent.Init(
            id, 
            name, 
            cost, 
            gives, 
            health, 
            damage, 
            distance, 
            count, 
            description, 
            icon
            );
        return buildComponent;
    }

    public UnitShopItem CreateUnit(ShopCacheItem ShopItemData, UnitCacheItem unitData)
    {
        string id = ShopItemData.GetExternalId();
        string name = unitData.GetName();
        ResourcesData cost = unitData.GetCost();
        ResourcesData gives = unitData.GetGives();
        int health = unitData.GetHealth();
        int damage = unitData.GetDamage();
        int distance = unitData.GetDistance();
        int mobility = unitData.GetMobility();
        int count = ShopItemData.GetCount();
        string description = unitData.GetDescrption();
        Sprite icon = Resources.Load<Sprite>(unitData.GetIconPath());

        GameObject itemObject = CreateObject(Config.resources["UI" + "Unit" + "ShopItemPrefab"], content);
        itemObject.name = name;
        UnitShopItem unitComponent = itemObject.GetComponent<UnitShopItem>();

        unitComponent.Init(
            id, 
            name, 
            cost,
            gives, 
            health, 
            damage, 
            distance, 
            mobility,
            count,
            description, 
            icon
            );
        return unitComponent;
    }

    public MaterialShopItem CreateMaterial(ShopCacheItem ShopItemData, MaterialCacheItem materialData)
    {
        string id = ShopItemData.GetExternalId();
        string name = materialData.GetName();
        ResourcesData cost = materialData.GetCost();
        int count = ShopItemData.GetCount();
        string description = materialData.GetDescrption();
        Sprite icon = Resources.Load<Sprite>(materialData.GetIconPath());

        GameObject itemObject = CreateObject(Config.resources["UI" + "Material" + "ShopItemPrefab"], content);
        itemObject.name = name;
        MaterialShopItem materialComponent = itemObject.GetComponent<MaterialShopItem>();

        materialComponent.Init(id, name, cost, count, description, icon);
        return materialComponent;
    }

    

    public void InitContent()
    {
        content = GameObject.FindGameObjectWithTag(Tags.shopItems).transform;
    }

    public void ClearItems()
    {
        GameObject[] children = content.gameObject.GetComponentsInChildren<Transform>(true)
            .Where(obj => obj != content)
            .Select(obj => obj.gameObject)
            .ToArray();

        foreach (GameObject child in children)
        {
            Destroy(child);
        }

        items = new List<ShopItem>();
    }
}
