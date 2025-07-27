using UnityEngine;


public class MissionItem : Item
{
    public static string type = "Mission";
    public override string Type { get { return type; } }

    public Bector2Int _position;
    public bool _isPassed = false;
    public ResourcesData _givesData;

    public MissionDetalization _detalization;

    public void Init(
        MissionDetalization missionDetalization,
        string missionId,
        string missionName,
        bool isPassed,
        ResourcesData missionGives,
        Bector2Int position = null,
        string missionDescription = ""
        )
    {
        _detalization = missionDetalization;

        _id = missionId; _objName = missionName; _isPassed = isPassed; _itemImage.sprite = _icon;
        _position = position;
        

        InitPosition();

        _givesData = missionGives;
        _description = missionDescription;

        InitIcon(_isPassed);
        UpdateUI();
    }

    public override void Awake()
    {
    }


    public void InitIcon(bool isPassed)
    {
        string iconName = isPassed == true ? "close" : "open";
        Sprite[] icons = Resources.LoadAll<Sprite>(Config.resources["missionIconsSection"]);
        foreach (Sprite icon in icons)
        {
            if (icon.name == iconName)
            {
                _icon = icon;
                _itemImage.sprite = icon;
                return;
            }
        }
    }

    public void InitPosition()
    {
        if (_position == null) return;
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(_position._x, _position._y);
    }

    public override void Interact()
    {
        ToggleDetalization();
        _detalization.UpdateDetalization(_id, _objName, _description, _givesData);
    }

    public void ToggleDetalization()
    {
        _detalization.gameObject.SetActive(!_detalization.gameObject.activeInHierarchy);
    }
}
