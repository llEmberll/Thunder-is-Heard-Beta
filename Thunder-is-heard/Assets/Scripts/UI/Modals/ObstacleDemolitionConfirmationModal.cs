using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class ObstacleDemolitionConfirmationModal : UIElement
{
    public bool focusOn = false;
    public TMP_Text description;

    public GameObject _obstacle;

    public ResourcesData costData;
    public Transform cost;

    public ResourcesProcessor resourcesProcessor;


    public void Start()
    {
        InitListeners();
        InitResourceProcessor();
        Hide();
    }

    public virtual void InitListeners()
    {
        EnableListeners();
    }

    public void InitResourceProcessor()
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag(Tags.resourcesProcessor).GetComponent<ResourcesProcessor>();
    }

    public virtual void EnableListeners()
    {
        EventMaster.current.ToggledToBuildMode += Hide;
        EventMaster.current.ObstacleDemolitionInitiated += UpdateTargetObstacle;
    }

    public virtual void DisableListeners()
    {
        EventMaster.current.ToggledToBuildMode -= Hide;
        EventMaster.current.ObstacleDemolitionInitiated -= UpdateTargetObstacle;
    }

    public void UpdateTargetObstacle(Obstacle obstacle)
    {
        Debug.Log("Demolition: update target");

        _obstacle = obstacle.gameObject;

        ObstacleCacheTable obstaclesTable = Cache.LoadByType<ObstacleCacheTable>();
        CacheItem cacheItem = obstaclesTable.GetById(obstacle.CoreId);
        if (cacheItem == null) return;

        ObstacleCacheItem obstacleCoreData = new ObstacleCacheItem(cacheItem.Fields);
        costData = obstacleCoreData.GetDemolitionCost();

        ResourcesProcessor.UpdateResources(cost, costData);

        string obstacleName = obstacle.name;
        string newDescription = Config.textPatterns["obstacleDemolitionConfirmation"];
        newDescription = newDescription.Replace("{name}", obstacleName);
        description.text = newDescription;

        Show();
    }

    public virtual void Update()
    {
        if (IsClickedOutside())
        {
            OnClickOutside();
        }
    }

    public virtual void OnClickOutside()
    {
        Hide();
    }

    public bool IsClickedOutside()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return !focusOn;
        }

        return false;
    }


    public void Confirm()
    {
        if (IsAvailableToBuy())
        {
            OnBuy();
            Hide();
        }
    }

    public bool IsAvailableToBuy()
    {
        return resourcesProcessor.IsAvailableToBuy(costData);
    }

    public void OnBuy()
    {
        ObjectProcessor.RemoveObjectFromBase(_obstacle);
        resourcesProcessor.SubstractResources(costData);
        resourcesProcessor.Save();
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        focusOn = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        focusOn = false;
    }

    public override void Hide()
    {
        base.Hide();
        EventMaster.current.OnUIPanelToggle(false);
    }

    public override void Show()
    {
        base.Show();
        EventMaster.current.OnUIPanelToggle(true);
    }
}
