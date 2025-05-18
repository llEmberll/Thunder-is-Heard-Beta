using UnityEngine.UI;


public class BuildingPanel : Panel
{
    public Image turnOnBuildingOption;
    public Image turnOffBuildingOption;
    public Image cancelOption;
    public Image rotateOption;
    public Image toInventoryOption;
    public Image sellOption;


    public static string ComponentType
    {
        get { return "BuildingPanel"; }
    }

    public ISubsituableBuildingOptionsBehaviour _behaviour;


    public void Start()
    {
        ChangeBehaviour();

        InitListeners();
    }

    public virtual void InitListeners()
    {
        EnableListeners();
    }

    public virtual void EnableListeners()
    {
        EventMaster.current.ToggledToBuildMode += OnBuildMode;
        EventMaster.current.ToggledOffBuildMode += OnExitBuildMode;
        EventMaster.current.ComponentBehaviourChanged += OnSomeComponentChangeBehaviour;
    }

    public virtual void DisableListeners()
    {
        EventMaster.current.ToggledToBuildMode -= OnBuildMode;
        EventMaster.current.ToggledOffBuildMode -= OnExitBuildMode;
        EventMaster.current.ComponentBehaviourChanged -= OnSomeComponentChangeBehaviour;
    }

    public void OnExitBuildMode()
    {
        _behaviour.OnExitBuildMode(this);
    }

    public void OnBuildMode()
    {
        _behaviour.OnBuildMode(this);
    }

    public void TurnOnAllOptionsExceptTurnOnBuilding()
    {
        turnOnBuildingOption.gameObject.SetActive(false);

        turnOffBuildingOption.gameObject.SetActive(true);
        cancelOption.gameObject.SetActive(true);
        rotateOption.gameObject.SetActive(true);
        toInventoryOption.gameObject.SetActive(true);
        sellOption.gameObject.SetActive(true);
    }

    public void TurnOffAllOptionsExceptTurnOnBuilding()
    {
        turnOnBuildingOption.gameObject.SetActive(true);

        turnOffBuildingOption.gameObject.SetActive(false);
        cancelOption.gameObject.SetActive(false);
        rotateOption.gameObject.SetActive(false);
        toInventoryOption.gameObject.SetActive(false);
        sellOption.gameObject.SetActive(false);
    }

    public void TurnOffOptions()
    {
        turnOnBuildingOption.gameObject.SetActive(false);
        turnOffBuildingOption.gameObject.SetActive(false);
        cancelOption.gameObject.SetActive(false);
        rotateOption.gameObject.SetActive(false);
        toInventoryOption.gameObject.SetActive(false);
        sellOption.gameObject.SetActive(false);
    }

    public void OnSomeComponentChangeBehaviour(string componentName, string behaviourName)
    {
        if (componentName != ComponentType) return;
        ChangeBehaviour(behaviourName);
    }

    public void OnResetBehaviour()
    {
        ChangeBehaviour();
    }

    public void ChangeBehaviour(string name = "Base")
    {
        _behaviour = SubsituableBuildingOptionsFactory.GetBehaviourById(name);
        _behaviour.Init(this);
    }
}
