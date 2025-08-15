using UnityEngine;


public class BuildingPanel : Panel
{
    public GameObject turnOnBuildingOption = null;
    public GameObject turnOffBuildingOption = null;
    public GameObject cancelOption = null;
    public GameObject rotateOption = null;
    public GameObject toInventoryOption = null;
    public GameObject sellOption = null;


    public static string ComponentType
    {
        get { return "BuildingPanel"; }
    }

    public ISubsituableBuildingOptionsBehaviour _behaviour;


    public void Start()
    {
        SceneState sceneState = GameObject.FindWithTag("State").GetComponent<SceneState>();
        bool isFight = sceneState.currentState.stateName == Scenes.fight;
        if (isFight)
        {
            ChangeBehaviour("Disabled");
        }
        else
        {
            ChangeBehaviour();
        }
        

        InitListeners();
    }

    public virtual void InitListeners()
    {
        EnableListeners();
    }

    public virtual void EnableListeners()
    {
        EventMaster.current.StartLanding += OnToggleToLanding;
        EventMaster.current.ToggledToBuildMode += OnBuildMode;
        EventMaster.current.ToggledOffBuildMode += OnExitBuildMode;
        EventMaster.current.ComponentBehaviourChanged += OnSomeComponentChangeBehaviour;
        EventMaster.current.ComponentsBehaviourReset += OnResetBehaviour;
        EventMaster.current.FightIsStarted += OnStartFight;
        EventMaster.current.FightIsContinued += OnStartFight;
    }

    public virtual void DisableListeners()
    {
        EventMaster.current.StartLanding -= OnToggleToLanding;
        EventMaster.current.ToggledToBuildMode -= OnBuildMode;
        EventMaster.current.ToggledOffBuildMode -= OnExitBuildMode;
        EventMaster.current.ComponentBehaviourChanged -= OnSomeComponentChangeBehaviour;
        EventMaster.current.ComponentsBehaviourReset -= OnResetBehaviour;
        EventMaster.current.FightIsStarted -= OnStartFight;
        EventMaster.current.FightIsContinued -= OnStartFight;
    }

    public void TurnOffBuilding()
    {
        _behaviour.TurnOffBuilding(this);
    }

    public virtual void TurnOnBuilding()
    {
        _behaviour.TurnOnBuilding(this);
    }

    public virtual void Cancel()
    {
        _behaviour.Cancel(this);
    }

    public virtual void Rotate()
    {
        _behaviour.Rotate(this);
    }

    public virtual void ToInventory()
    {
        _behaviour.ToInventory(this);
    }

    public virtual void Sell()
    {
        _behaviour.Sell(this);
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
        ToggleObjects(
            objs: new GameObject[] { turnOnBuildingOption }, 
            active: false
        );

        ToggleObjects(
            objs: new GameObject[] {
                turnOffBuildingOption,
                cancelOption,
                rotateOption,
                toInventoryOption,
                sellOption
            },
            active: true
        );
    }

    public void TurnOffAllOptionsExceptTurnOnBuilding()
    {
        ToggleObjects(
            objs: new GameObject[] { turnOnBuildingOption },
            active: true
        );

        ToggleObjects(
            objs: new GameObject[] {
                turnOffBuildingOption,
                cancelOption,
                rotateOption,
                toInventoryOption,
                sellOption
            },
            active: false
        );
    }

    public void TurnOffAllOptions()
    {
        ToggleObjects(
            objs: new GameObject[] {
                turnOnBuildingOption,
                turnOffBuildingOption,
                cancelOption,
                rotateOption,
                toInventoryOption,
                sellOption
            },
            active: false
        );
    }

    public void ToggleObjects(GameObject[] objs, bool active)
    {
        foreach (GameObject obj in objs)
        {
            if (obj != null)
            {
                obj.SetActive(active);
            }
        }
    }

    public void OnSomeComponentChangeBehaviour(string componentName, string behaviourName)
    {
        if (componentName != ComponentType) return;
        ChangeBehaviour(behaviourName);
    }

    public void OnToggleToLanding(LandingData landingData)
    {
        ChangeBehaviour();
    }

    public void OnStartFight()
    {
        ChangeBehaviour("Disabled");
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
