using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class FocusController : MonoBehaviour
{
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
        areaHighlightCanvas = GameObject.FindGameObjectWithTag(Tags.areaHighlightCanvas).GetComponent<Canvas>();
        areaHighlightCanvas.enabled = false;
    }


    public virtual void OnFocus(FocusData focusData)
    {
        string targetType = focusData.Type;
        switch (targetType)
        {
            case "Build":
                OnBuildFocus(focusData.Data);
                return;
            case "Button":
                OnButtonFocus(focusData.Data);
                return;
            case "UIItem":
                OnUIItemFocus(focusData.Data);
                return;
            case "Text":
                OnTextFocus(focusData.Data);
                return;
            case "Area":
                OnAreaFocus(focusData.Data);
                return;
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
        if (!data.ContainsKey("rectangle"))
        {
            throw new System.Exception("Not sent rectangle when focus on area");
        }

        RectangleBector2Int areaAsRectangle = (RectangleBector2Int)data["rectangle"];
        Bector2Int center = areaAsRectangle.FindCenter();

        SetCameraFocus(center.ToVector2Int(), data);

        if (data.ContainsKey("visible") && (bool)data["visible"] == true)
        {
            конфигурация areaCanvas
        }
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
