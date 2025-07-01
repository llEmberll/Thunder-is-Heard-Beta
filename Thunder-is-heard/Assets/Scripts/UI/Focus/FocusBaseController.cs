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
        builds = GameObject.FindGameObjectWithTag(Tags.buildsOnScene).GetComponent<BuildsOnBase>();
        base.Awake();
    }

    public override void InitButtons()
    {
        buttonImageByTag = new Dictionary<string, Image>();

        Image shopButtonImage = GameObject.FindGameObjectWithTag(Tags.toShopButton).GetComponent<Image>();
        buttonImageByTag.Add(Tags.toShopButton, shopButtonImage);

        Image inventoryButtonImage = GameObject.FindGameObjectWithTag(Tags.toInventoryButton).GetComponent<Image>();
        buttonImageByTag.Add(Tags.toInventoryButton, inventoryButtonImage);

        Image campanyButtonImage = GameObject.FindGameObjectWithTag(Tags.toCampaignButton).GetComponent<Image>();
        buttonImageByTag.Add(Tags.toCampaignButton, campanyButtonImage);

        Image battlefieldButtonImage = GameObject.FindGameObjectWithTag(Tags.toBattlefieldButton).GetComponent<Image>();
        buttonImageByTag.Add(Tags.toBattlefieldButton, battlefieldButtonImage);

        Image PVPButtonImage = GameObject.FindGameObjectWithTag(Tags.toPVPButton).GetComponent<Image>();
        buttonImageByTag.Add(Tags.toPVPButton, PVPButtonImage);

        Image mailButton = GameObject.FindGameObjectWithTag(Tags.toMailButton).GetComponent<Image>();
        buttonImageByTag.Add(Tags.toMailButton, mailButton);

        Image reportsButtonImage = GameObject.FindGameObjectWithTag(Tags.toReportsButton).GetComponent<Image>();
        buttonImageByTag.Add(Tags.toReportsButton, reportsButtonImage);
    }

    public override void InitUI()
    {
        _shopUI = GameObject.FindGameObjectWithTag(Tags.shop).GetComponent<Shop>();
        _inventoryUI = GameObject.FindGameObjectWithTag(Tags.inventory).GetComponent<Inventory>();
        _contractsUI = GameObject.FindGameObjectWithTag(Tags.contracts).GetComponent<Contracts>();
        _unitProductionsUI = GameObject.FindGameObjectWithTag(Tags.unitProductions).GetComponent<UnitProductions>();
        _campanyUI = GameObject.FindGameObjectWithTag(Tags.campany).GetComponent<Campany>();
    }

    public override void InitTexts()
    {
        textByTag = new Dictionary<string, TMP_Text>();

        TMP_Text baseNameText = GameObject.FindGameObjectWithTag(Tags.renameBaseButton).GetComponent<TMP_Text>();
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
        EventMaster.current.FocusCameraOnPosition(sourceBuild.center, true);

        _targetEntity = sourceBuild;
        SaveMaterials(sourceBuild.gameObject);
    }
}
