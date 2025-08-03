using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FocusBaseController : FocusController
{
    public BuildsOnBase builds;

    public Shop _shopUI;
    public Inventory _inventoryUI;
    public Campany _campanyUI;
    public Contracts _contractsUI;
    public UnitProductions _unitProductionsUI;


    public override void Awake()
    {
        builds = GameObjectUtils.FindComponentByTagIncludingInactive<BuildsOnBase>(Tags.buildsOnScene);
        base.Awake();
    }

    public override void InitButtons()
    {
        buttonImageByTag = new Dictionary<string, Image>();

        Image shopButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.toShopButton);
        buttonImageByTag.Add(Tags.toShopButton, shopButtonImage);

        Image inventoryButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.toInventoryButton);
        buttonImageByTag.Add(Tags.toInventoryButton, inventoryButtonImage);

        Image campanyButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.toCampaignButton);
        buttonImageByTag.Add(Tags.toCampaignButton, campanyButtonImage);

        Image battlefieldButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.toBattlefieldButton);
        buttonImageByTag.Add(Tags.toBattlefieldButton, battlefieldButtonImage);

        Image PVPButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.toPVPButton);
        buttonImageByTag.Add(Tags.toPVPButton, PVPButtonImage);

        Image mailButton = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.toMailButton);
        buttonImageByTag.Add(Tags.toMailButton, mailButton);

        Image reportsButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.toReportsButton);
        buttonImageByTag.Add(Tags.toReportsButton, reportsButtonImage);
    }

    public override void InitUI()
    {
        _shopUI = GameObjectUtils.FindComponentByTagIncludingInactive<Shop>(Tags.shop);
        _inventoryUI = GameObjectUtils.FindComponentByTagIncludingInactive<Inventory>(Tags.inventory);
        _contractsUI = GameObjectUtils.FindComponentByTagIncludingInactive<Contracts>(Tags.contracts);
        _unitProductionsUI = GameObjectUtils.FindComponentByTagIncludingInactive<UnitProductions>(Tags.unitProductions);
        _campanyUI = GameObjectUtils.FindComponentByTagIncludingInactive<Campany>(Tags.campany);
    }

    public override void InitTexts()
    {
        textByTag = new Dictionary<string, TMP_Text>();

        TMP_Text baseNameText = GameObjectUtils.FindComponentByTagIncludingInactive<TMP_Text>(Tags.renameBaseButton);
        textByTag.Add(Tags.renameBaseButton, baseNameText);
    }

    public override void OnFocus(FocusData focusData)
    {
        base.OnFocus(focusData);

        string targetType = focusData.Type;
        switch (targetType)
        {
            case "ProductsNotification":
                OnProductsNotificationFocus(focusData.Data);
                break;
            default:
                Debug.Log("Unexpected focus type: " + targetType);
                return;
        }
    }

    public override Build FindBuildByFocusData(Dictionary<string, object> data)
    {
        Build build = null;
        if (data.ContainsKey("childId"))
        {
            string childId = (string)data["childId"];
            build = builds.FindObjectByChildId(childId) as Build;
        }
        else if (data.ContainsKey("coreId"))
        {
            string coreId = (string)data["coreId"];
            build = builds.FindObjectByCoreId(coreId) as Build;
        }

        return build;
    }

    public override void OnUIItemFocus(Dictionary<string, object> data)
    {
        string type = (string)data["UIType"];

        switch (type)
        {
            case "Shop":
                OnShopItemFocus(data);
                break;
            case "Inventory":
                OnInventoryItemFocus(data);
                break;
            case "UnitProductions":
                OnUnitProductionsItemFocus(data);
                break;
            case "Contracts":
                OnContractItemFocus(data);
                break;
            case "Campany":
                OnCampanyItemFocus(data);
                break;
            default:
                throw new System.Exception("Undefined UIType: " +  type);
        }
    }

    public void OnShopItemFocus(Dictionary<string, object> data)
    {
        ShopItem item = null;
        if (data.ContainsKey("itemId"))
        {
            string itemId = (string)data["itemId"];
            item = _shopUI.FindItemById(itemId);
        }
        else if (data.ContainsKey("coreId"))
        {
            string coreId = (string)data["coreId"];
            item  =_shopUI.FindItemByCoreId(coreId);
        }

        if (item == null) {
            Debug.Log("Focus on UIItem: item not found: " + data);
            return; 
        }

        Image itemImage = item.gameObject.GetComponent<Image>();
        _targetImage = itemImage;
    }

    public void OnInventoryItemFocus(Dictionary<string, object> data)
    {
        InventoryItem item = null;
        if (data.ContainsKey("itemId"))
        {
            string itemId = (string)data["itemId"];
            item = _inventoryUI.FindItemById(itemId);
        }
        else if (data.ContainsKey("coreId"))
        {
            string coreId = (string)data["coreId"];
            item = _inventoryUI.FindItemByCoreId(coreId);
        }

        if (item == null)
        {
            Debug.Log("Focus on UIItem: item not found: " + data);
            return;
        }

        Image itemImage = item.gameObject.GetComponent<Image>();
        _targetImage = itemImage;
    }

    public void OnContractItemFocus(Dictionary<string, object> data)
    {
        ContractItem item = null;
        if (data.ContainsKey("itemId"))
        {
            string itemId = (string)data["itemId"];
            item = _contractsUI.FindItemById(itemId);
        }
        else if (data.ContainsKey("contractType"))
        {
            string contractType = (string)data["contractType"];
            item = _contractsUI.FindItemByType(contractType);
        }

        if (item == null)
        {
            Debug.Log("Focus on UIItem: item not found: " + data);
            return;
        }

        Image itemImage = item.gameObject.GetComponent<Image>();
        _targetImage = itemImage;
    }

    public void OnUnitProductionsItemFocus(Dictionary<string, object> data)
    {
        UnitProductionItem item = null;
        if (data.ContainsKey("itemId"))
        {
            string itemId = (string)data["itemId"];
            item = _unitProductionsUI.FindItemById(itemId);
        }
        else if (data.ContainsKey("unitId"))
        {
            string unitId = (string)data["unitId"];
            item = _unitProductionsUI.FindItemByUnitId(unitId);
        }
        else if (data.ContainsKey("unitType"))
        {
            string unitType = (string)data["unitType"];
            item = _unitProductionsUI.FindItemByUnitType(unitType);
        }

        if (item == null)
        {
            Debug.Log("Focus on UIItem: item not found: " + data);
            return;
        }

        Image itemImage = item.gameObject.GetComponent<Image>();
        _targetImage = itemImage;
    }

    public void OnCampanyItemFocus(Dictionary<string, object> data)
    {
        MissionItem item = null;
        if (data.ContainsKey("itemId"))
        {
            string itemId = (string)data["itemId"];
            item = _campanyUI.FindItemById(itemId);
        }

        if (item == null)
        {
            Debug.Log("Focus on UIItem: item not found: " + data);
            return;
        }

        Image itemImage = item.gameObject.GetComponent<Image>();
        _targetImage = itemImage;
    }

    public void OnProductsNotificationFocus(Dictionary<string, object> data)
    {
        ProductsNotificationCacheTable productsNotificationCacheTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        ProductsNotificationCacheItem productsNotificationData = null;

        if (data.ContainsKey("notificationId"))
        {
            string id = (string)data["notificationId"];
            CacheItem cacheItem = productsNotificationCacheTable.GetById(id);
            productsNotificationData = new ProductsNotificationCacheItem(cacheItem.Fields);
        }
        else if (data.ContainsKey("type"))
        {
            string type = (string)data["type"];
            productsNotificationData = productsNotificationCacheTable.FindByType(type);
        }
        else
        {
            Debug.Log("Focus on productsNotification: invalid data: " + data);
            return;
        }

        if (productsNotificationData == null)
        {
            Debug.Log("Focus on productsNotification: can't find notification. Data: " + data);
            return;
        }

        Build sourceBuild = builds.FindObjectByChildId(productsNotificationData.GetSourceObjectId()) as Build;

        SetCameraFocus(sourceBuild.center, data);

        _targetEntity = sourceBuild;
        SaveMaterials(sourceBuild.gameObject);
    }
}
