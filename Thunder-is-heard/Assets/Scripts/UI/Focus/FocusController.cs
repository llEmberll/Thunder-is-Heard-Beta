using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FocusController : MonoBehaviour
{
    public BuildsOnBase builds;

    private float _blinkProgress = 0f; //  (от 0 до 1)
    private const float BLINK_DURATION = 1f; // длительность периода (1 секунда)

    public int _minMaterialColorLevel = 110;
    public int _maxMaterialColorLevel = 255;
    public int _maxChangeFromOriginalColorLevel = 145;
    public int _current_ChangeFromOriginalColorLevel = 0;
    public Dictionary<string, int> _defaultMaterialColorLevelByName = new Dictionary<string, int>();
    public Dictionary<string, Material> _targetObjectMaterials = null;


    public int _originalImageTransparent = 255;
    public int _minImageTransparent = 110;
    public int _maxImageTransparent = 255;
    public Image _targetImage = null;
    public TMP_Text _targetText = null;

    public Dictionary<string, Image> buttonImageByTag;

    public Shop _shopUI;
    public Inventory _inventoryUI;
    public Campany _campanyUI;
    public Contracts _contractsUI;
    public UnitProductions _unitProductionsUI;

    public Dictionary<string, TMP_Text> textByTag;

    // Для мигания текста
    private Color? _originalTextColor = null;
    public int _minTextAlpha = 110;
    public int _maxTextAlpha = 255;

    public void Awake()
    {
        builds = GameObject.FindGameObjectWithTag(Tags.buildsOnScene).GetComponent<BuildsOnBase>();
        InitButtons();
        InitUI();
        InitTexts();
    }

    public void Start()
    {
        EventMaster.current.ObjectFocused += OnFocus;
        EventMaster.current.ClearObjectFocused += OnDefocus;

    }

    public void InitButtons()
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

    public void InitUI()
    {
        _shopUI = GameObject.FindGameObjectWithTag(Tags.shop).GetComponent<Shop>();
        _inventoryUI = GameObject.FindGameObjectWithTag(Tags.inventory).GetComponent<Inventory>();
        _contractsUI = GameObject.FindGameObjectWithTag(Tags.contracts).GetComponent<Contracts>();
        _unitProductionsUI = GameObject.FindGameObjectWithTag(Tags.unitProductions).GetComponent<UnitProductions>();
        _campanyUI = GameObject.FindGameObjectWithTag(Tags.campany).GetComponent<Campany>();
    }

    public void InitTexts()
    {
        textByTag = new Dictionary<string, TMP_Text>();

        TMP_Text baseNameText = GameObject.FindGameObjectWithTag(Tags.renameBaseButton).GetComponent<TMP_Text>();
        textByTag.Add(Tags.renameBaseButton, baseNameText);
    }

    public void Update()
    {
        if (_targetImage != null)
        {
            ProcessImageTarget();
        }
        else if (_targetObjectMaterials != null)
        {
            ProcessTargetWithMaterials();
        }

        else if (_targetText != null)
        {
            ProcessTextTarget();
        }
    }


    public void OnFocus(FocusData focusData)
    {
        string targetType = focusData.Type;
        switch (targetType)
        {
            case "Build":
                OnBuildFocus(focusData.Data);
                break;
            case "Button":
                OnButtonFocus(focusData.Data);
                break;
            case "UIItem":
                OnUIItemFocus(focusData.Data);
                break;
            case "ProductsNotification":
                OnProductsNotificationFocus(focusData.Data);
                break;
            case "Text":
                OnTextFocus(focusData.Data);
                break;
            default:
                Debug.Log("Unexpected focus type: " +  targetType);
                break;
        }
    }

    public void OnDefocus()
    {
        Debug.Log("Focus controller  unlock camera, camera ON");

        EventMaster.current.CancelFocus();

        ResetMaterials();
        ResetImage();
        ResetText();

        _blinkProgress = 0;
        _current_ChangeFromOriginalColorLevel = 0;
        _defaultMaterialColorLevelByName = new Dictionary<string, int>();
        _targetObjectMaterials = null;
        _targetImage = null;
    }

    public void OnBuildFocus(Dictionary<string, object> data)
    {
        Build build = null;
        if (data.ContainsKey("childId")) 
        {
            string childId = (string)data["childId"];
            build = builds.FindObjectByChildId(childId) as Build;
            if (build == null) return;
        }
        else if (data.ContainsKey("coreId"))
        {
            string coreId = (string)data["coreId"];
            build = builds.FindObjectByCoreId(coreId) as Build;
            if (build == null) return;
        }
        if (build == null) return;


        EventMaster.current.FocusCameraOnPosition(build.center, true);

        SaveMaterials(build.gameObject);
    }

    public void SaveMaterials(GameObject obj)
    {
        _targetObjectMaterials = new Dictionary<string, Material>();
        SaveMaterialsRecursive(obj);
    }

    public void SaveMaterialsRecursive(GameObject obj)
    {
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material[] materials = meshRenderer.materials;
            foreach (Material mat in materials)
            {
                if (!_targetObjectMaterials.ContainsKey(mat.name))
                {
                    _targetObjectMaterials.Add(mat.name, mat);
                }

                if (!_defaultMaterialColorLevelByName.ContainsKey(mat.name))
                {
                    _defaultMaterialColorLevelByName[mat.name] = (int)(mat.color.grayscale * 255);
                }
            }
        }

        else
        {
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null)
            {
                Material[] materials = skinnedMeshRenderer.materials;
                foreach (Material mat in materials)
                {
                    if (!_targetObjectMaterials.ContainsKey(mat.name))
                    {
                        _targetObjectMaterials.Add(mat.name, mat);
                    }

                    if (!_defaultMaterialColorLevelByName.ContainsKey(mat.name))
                    {
                        _defaultMaterialColorLevelByName[mat.name] = (int)(mat.color.grayscale * 255);
                    }
                }
            }
        }

        foreach (Transform child in obj.transform)
        {
            SaveMaterialsRecursive(child.gameObject);
        }
    }

    public void OnButtonFocus(Dictionary<string, object> data)
    {
        if (!data.ContainsKey("tag"))
        {
            throw new System.Exception("Focus on button: tag not present in data");
        }
        string buttonTag = (string)data["tag"];
        Image buttonImage = buttonImageByTag[buttonTag];
        _targetImage = buttonImage;
    }

    public void OnUIItemFocus(Dictionary<string, object> data)
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

        SaveMaterials(sourceBuild.gameObject);
    }

    public void OnTextFocus(Dictionary<string, object> data)
    {
        if (!data.ContainsKey("tag"))
        {
            throw new System.Exception("Focus on text: tag not present in data");
        }
        string textTag = (string)data["tag"];
        TMP_Text text = textByTag[textTag];
        _targetText = text;
    }

    public void ProcessTargetWithMaterials()
    {
        if (_targetObjectMaterials == null || _targetObjectMaterials.Count == 0) return;

        _blinkProgress += Time.deltaTime / BLINK_DURATION;
        _blinkProgress %= 1f;

        float minBrightness = _minMaterialColorLevel / 255f;
        float maxBrightness = _maxMaterialColorLevel / 255f;
        float newBrightness = Mathf.Lerp(minBrightness, maxBrightness, Mathf.PingPong(_blinkProgress, 1f));

        foreach (var materialPair in _targetObjectMaterials)
        {
            Material material = materialPair.Value;
            Color originalColor = material.color;
            
            material.color = new Color(
                newBrightness,
                newBrightness,
                newBrightness,
                originalColor.a
            );
        }
    }

    public void ProcessImageTarget()
    {
        if (_targetImage == null) return;

        _blinkProgress += Time.deltaTime / BLINK_DURATION;
        _blinkProgress %= 1f;

        float minAlpha = _minImageTransparent / 255f;
        float maxAlpha = _maxImageTransparent / 255f;
        float newAlpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(_blinkProgress, 1f));

        _targetImage.color = new Color(
            _targetImage.color.r,
            _targetImage.color.g,
            _targetImage.color.b,
            newAlpha
        );
    }

    public void ProcessTextTarget()
    {
        if (_targetText == null) return;

        // Сохраняем исходный цвет, если ещё не сохранён
        if (_originalTextColor == null)
        {
            _originalTextColor = _targetText.color;
        }

        // Логика мигания альфа-канала текста
        _blinkProgress += Time.deltaTime / BLINK_DURATION;
        _blinkProgress %= 1f;

        float originalAlpha = ((Color)_originalTextColor).a;
        float minAlpha = _minTextAlpha / 255f;
        float maxAlpha = _maxTextAlpha / 255f;
        float targetAlpha = Mathf.Clamp01(originalAlpha - _current_ChangeFromOriginalColorLevel / 255f);

        float newAlpha = Mathf.Lerp(
            originalAlpha,
            targetAlpha,
            Mathf.PingPong(_blinkProgress, 1f)
        );
        newAlpha = Mathf.Clamp(newAlpha, minAlpha, maxAlpha);

        Color c = _targetText.color;
        c.a = newAlpha;
        _targetText.color = c;
    }

    public void ResetMaterials()
    {
        if (_targetObjectMaterials != null)
        {
            foreach (var materialPair in _targetObjectMaterials)
            {
                Material material = materialPair.Value;
                if (_defaultMaterialColorLevelByName.ContainsKey(material.name))
                {
                    int originalColorLevel = _defaultMaterialColorLevelByName[material.name];
                    material.color = new Color(
                        originalColorLevel / 255f,
                        originalColorLevel / 255f,
                        originalColorLevel / 255f,
                        material.color.a
                    );
                }
            }
        }
    }

    public void ResetImage()
    {
        if (_targetImage != null)
        {
            _targetImage.color = new Color(
                _targetImage.color.r,
                _targetImage.color.g,
                _targetImage.color.b,
                _originalImageTransparent / 255f
            );
        }
    }

    public void ResetText()
    {
        if (_targetText != null && _originalTextColor != null)
        {
            _targetText.color = (Color)_originalTextColor;
        }
        _originalTextColor = null;
    }
}
