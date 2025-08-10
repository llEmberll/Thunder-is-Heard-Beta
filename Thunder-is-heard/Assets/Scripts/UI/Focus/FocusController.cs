using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public abstract class FocusController : MonoBehaviour
{
    private float _blinkProgress = 0f; //  (от 0 до 1)
    private const float BLINK_DURATION = 1f; // длительность периода (1 секунда)

    public int _minMaterialColorLevel = 110;
    public int _maxMaterialColorLevel = 255;
    public int _maxChangeFromOriginalColorLevel = 145;
    public int _current_ChangeFromOriginalColorLevel = 0;
    // Словарь для хранения оригинальных уровней яркости материалов по уникальным ключам (InstanceID_имяМатериала)
    public Dictionary<string, int> _defaultMaterialColorLevelByName = new Dictionary<string, int>();
    // Словарь для хранения материалов по уникальным ключам (InstanceID_имяМатериала)
    public Dictionary<string, Material> _targetObjectMaterials = null;


    public int _originalImageTransparent = 255;
    public int _minImageTransparent = 110;
    public int _maxImageTransparent = 255;
    public Image _targetImage = null;
    public TMP_Text _targetText = null;

    public Dictionary<string, Image> buttonImageByTag;

    public Dictionary<string, TMP_Text> textByTag;

    public Canvas areaHighlightCanvas;
    public int areaHighlightSizePerCell = 800;

    // Для мигания текста
    private Color? _originalTextColor = null;
    public int _minTextAlpha = 110;
    public int _maxTextAlpha = 255;

    // Свойство для хранения целевой сущности (юнит или здание)
    public Entity _targetEntity = null;

    public virtual void Awake()
    {
        InitButtons();
        InitUI();
        InitTexts();
        InitAreaHighlight();
    }

    public void Start()
    {
        EventMaster.current.ObjectFocused += OnFocus;
        EventMaster.current.ClearObjectFocused += OnDefocus;

    }

    public abstract void InitButtons();

    public abstract void InitUI();

    public abstract void InitTexts();

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

    public void InitAreaHighlight()
    {
        areaHighlightCanvas = GameObjectUtils.FindComponentByTagIncludingInactive<Canvas>(Tags.areaHighlightCanvas);
        areaHighlightCanvas.enabled = false;
    }


    public virtual void OnFocus(FocusData focusData)
    {
        if (focusData == null) return;
        
        Debug.Log($"[FocusController] OnFocus вызван с типом: {focusData.Type}");
        Debug.Log($"[FocusController] FocusData.Data тип: {focusData.Data?.GetType()}");
        
        switch (focusData.Type)
        {
            case "Area":
                OnAreaFocus(focusData.Data);
                break;
            case "Build":
                OnBuildFocus(focusData.Data);
                break;
            case "Button":
                OnButtonFocus(focusData.Data);
                break;
            case "UIItem":
                OnUIItemFocus(focusData.Data);
                break;
            case "Text":
                OnTextFocus(focusData.Data);
                break;
            default:
                Debug.LogWarning($"[FocusController] Неизвестный тип фокуса: {focusData.Type}");
                break;
        }
    }

    /// <summary>
    /// Рекурсивно преобразует JObject в Dictionary<string, object>
    /// </summary>
    private Dictionary<string, object> ConvertJObjectToDictionary(Newtonsoft.Json.Linq.JObject jObject)
    {
        var result = new Dictionary<string, object>();
        
        foreach (var property in jObject.Properties())
        {
            var value = property.Value;
            result[property.Name] = ConvertJTokenToObject(value);
        }
        
        return result;
    }
    
    /// <summary>
    /// Преобразует JToken в объект, рекурсивно обрабатывая вложенные структуры
    /// </summary>
    private object ConvertJTokenToObject(Newtonsoft.Json.Linq.JToken token)
    {
        if (token == null) return null;
        
        switch (token.Type)
        {
            case Newtonsoft.Json.Linq.JTokenType.Object:
                return ConvertJObjectToDictionary((Newtonsoft.Json.Linq.JObject)token);
                
            case Newtonsoft.Json.Linq.JTokenType.Array:
                var array = new List<object>();
                foreach (var item in token)
                {
                    array.Add(ConvertJTokenToObject(item));
                }
                return array.ToArray();
                
            case Newtonsoft.Json.Linq.JTokenType.String:
                return token.ToString();
                
            case Newtonsoft.Json.Linq.JTokenType.Integer:
                return Convert.ToInt32(token);
                
            case Newtonsoft.Json.Linq.JTokenType.Float:
                return Convert.ToSingle(token);
                
            case Newtonsoft.Json.Linq.JTokenType.Boolean:
                return Convert.ToBoolean(token);
                
            case Newtonsoft.Json.Linq.JTokenType.Null:
                return null;
                
            default:
                return token.ToString();
        }
    }

    public void OnDefocus()
    {
        EventMaster.current.CancelFocus();

        ResetMaterials();
        ResetImage();
        ResetText();

        _blinkProgress = 0;
        _current_ChangeFromOriginalColorLevel = 0;
        _defaultMaterialColorLevelByName = new Dictionary<string, int>();
        _targetObjectMaterials = null;
        _targetImage = null;
        _targetText = null;
        _targetEntity = null;

        areaHighlightCanvas.enabled = false;
    }

    public virtual void OnBuildFocus(Dictionary<string, object> data)
    {
        Build build = FindBuildByFocusData(data);
        if (build == null) return;

        SetCameraFocus(build.center, data);

        _targetEntity = build;
        SaveMaterials(build.gameObject);
    }

    public virtual void SetCameraFocus(Vector2Int position, Dictionary<string, object> data)
    {
        bool lockCamera = true;
        if (data.ContainsKey("lockCamera"))
        {
            lockCamera = (bool)data["lockCamera"];
        }

        EventMaster.current.FocusCameraOnPosition(position, lockCamera: lockCamera);
    }

    public abstract Build FindBuildByFocusData(Dictionary<string, object> data);

    public void SaveMaterials(GameObject obj)
    {
        _targetObjectMaterials = new Dictionary<string, Material>();
        SaveMaterialsRecursive(obj);
    }

    /// <summary>
    /// Рекурсивно сохраняет материалы объекта и всех его дочерних объектов.
    /// Использует InstanceID объекта + имя материала для создания уникальных ключей,
    /// что позволяет корректно обрабатывать объекты с одинаковыми именами материалов.
    /// </summary>
    /// <param name="obj">GameObject для сохранения материалов</param>
    public void SaveMaterialsRecursive(GameObject obj)
    {
        int objectId = obj.GetInstanceID();
        
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material[] materials = meshRenderer.materials;
            foreach (Material mat in materials)
            {
                string uniqueKey = $"{objectId}_{mat.name}";
                if (!_targetObjectMaterials.ContainsKey(uniqueKey))
                {
                    _targetObjectMaterials.Add(uniqueKey, mat);
                }

                if (!_defaultMaterialColorLevelByName.ContainsKey(uniqueKey))
                {
                    _defaultMaterialColorLevelByName[uniqueKey] = (int)(mat.color.grayscale * 255);
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
                    string uniqueKey = $"{objectId}_{mat.name}";
                    if (!_targetObjectMaterials.ContainsKey(uniqueKey))
                    {
                        _targetObjectMaterials.Add(uniqueKey, mat);
                    }

                    if (!_defaultMaterialColorLevelByName.ContainsKey(uniqueKey))
                    {
                        _defaultMaterialColorLevelByName[uniqueKey] = (int)(mat.color.grayscale * 255);
                    }
                }
            }
        }

        foreach (Transform child in obj.transform)
        {
            SaveMaterialsRecursive(child.gameObject);
        }
    }

    public virtual void OnButtonFocus(Dictionary<string, object> data)
    {
        if (!data.ContainsKey("tag"))
        {
            throw new System.Exception("Focus on button: tag not present in data");
        }
        string buttonTag = (string)data["tag"];
        Image buttonImage = buttonImageByTag[buttonTag];
        _targetImage = buttonImage;
    }

    public abstract void OnUIItemFocus(Dictionary<string, object> data);

    public virtual void OnTextFocus(Dictionary<string, object> data)
    {
        if (!data.ContainsKey("tag"))
        {
            throw new System.Exception("Focus on text: tag not present in data");
        }
        string textTag = (string)data["tag"];
        TMP_Text text = textByTag[textTag];
        _targetText = text;
        _originalTextColor = text.color;
    }

    public virtual void OnAreaFocus(Dictionary<string, object> data)
    {
        Debug.Log($"[FocusController] OnAreaFocus вызван с данными: {string.Join(", ", data.Select(kv => $"{kv.Key}={kv.Value}"))}");
        
        if (!data.ContainsKey("rectangle"))
        {
            Debug.LogError("[FocusController] Ключ 'rectangle' отсутствует в данных");
            throw new System.Exception("Not sent rectangle when focus on area");
        }

        // Получаем rectangle данные как JObject
        var rectangleJObject = data["rectangle"] as Newtonsoft.Json.Linq.JObject;
        if (rectangleJObject == null)
        {
            Debug.LogError($"[FocusController] Не удалось привести rectangle к JObject. Тип: {data["rectangle"]?.GetType()}");
            throw new System.Exception("Rectangle data is not in expected format");
        }
        
        Debug.Log($"[FocusController] RectangleData получен как JObject. Свойства: {string.Join(", ", rectangleJObject.Properties().Select(p => p.Name))}");
        
        // Проверяем наличие обязательных свойств
        if (!rectangleJObject.ContainsKey("startPosition") || !rectangleJObject.ContainsKey("size"))
        {
            Debug.LogError("[FocusController] Отсутствуют обязательные свойства 'startPosition' или 'size' в rectangleJObject");
            throw new System.Exception("Rectangle data missing startPosition or size");
        }
        
        // Получаем startPosition и size как JObject
        var startPositionJObject = rectangleJObject["startPosition"] as Newtonsoft.Json.Linq.JObject;
        var sizeJObject = rectangleJObject["size"] as Newtonsoft.Json.Linq.JObject;
        
        if (startPositionJObject == null || sizeJObject == null)
        {
            Debug.LogError($"[FocusController] startPositionJObject или sizeJObject null. startPositionJObject: {startPositionJObject}, sizeJObject: {sizeJObject}");
            throw new System.Exception("Rectangle data missing startPosition or size");
        }

        // Извлекаем координаты из JObject
        var startX = startPositionJObject["x"];
        var startY = startPositionJObject["y"];
        var sizeX = sizeJObject["x"];
        var sizeY = sizeJObject["y"];

        Debug.Log($"[FocusController] Извлеченные значения: startX={startX}, startY={startY}, sizeX={sizeX}, sizeY={sizeY}");

        var areaAsRectangle = RectangleBector2Int.FromStartAndSize(
            new Bector2Int(
                Convert.ToInt32(startX), 
                Convert.ToInt32(startY)
            ),
            new Bector2Int(
                Convert.ToInt32(sizeX), 
                Convert.ToInt32(sizeY)
            )
        );
        
        Debug.Log($"[FocusController] RectangleBector2Int успешно создан: {areaAsRectangle}");
        Vector2Int centerInteger = areaAsRectangle.FindAbsoluteCenterAsInt();

        SetCameraFocus(centerInteger, data);

        if (data.ContainsKey("visible") && (bool)data["visible"] == true)
        {
            ConfigureAreaCanvas(areaAsRectangle);
        }
    }

    public void ConfigureAreaCanvas(RectangleBector2Int areaAsRectangle) 
    {
        Vector2 centerAbsolute = areaAsRectangle.FindAbsoluteCenter();
        areaHighlightCanvas.transform.position = new Vector3(centerAbsolute.x, areaHighlightCanvas.transform.position.y, centerAbsolute.y);
        areaHighlightCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(areaAsRectangle._size._x * areaHighlightSizePerCell, areaAsRectangle._size._y * areaHighlightSizePerCell);
        areaHighlightCanvas.enabled = true;
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

        _blinkProgress += Time.deltaTime / BLINK_DURATION;
        _blinkProgress %= 1f;

        float minAlpha = _minTextAlpha / 255f;
        float maxAlpha = _maxTextAlpha / 255f;
        float newAlpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(_blinkProgress, 1f));

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
                string key = materialPair.Key;
                Material material = materialPair.Value;
                if (_defaultMaterialColorLevelByName.ContainsKey(key))
                {
                    int originalColorLevel = _defaultMaterialColorLevelByName[key];
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
