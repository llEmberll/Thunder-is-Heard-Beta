using UnityEngine.UI;

public class FightPanel : Panel
{

    public string Type
    {
        get
        {
            return "FightPanel";
        }
    }

    public Image fightButtons;
    public Image prepareButtons;

    public ISubsituableFightOptionsBehaviour _behaviour;

    public void Awake()
    {
        EnableListeners();
    }

    public void Start()
    {
        ChangeBehaviour();
    }

    public void EnableListeners()
    {
        EventMaster.current.StartLanding += ToggleToLanding;
        EventMaster.current.FightIsStarted += ToggleToFight;
        EventMaster.current.FightIsContinued += ToggleToFight;
    }

    public void DisableListeners()
    {
        EventMaster.current.StartLanding -= ToggleToLanding;
        EventMaster.current.FightIsStarted -= ToggleToFight;
        EventMaster.current.FightIsContinued -= ToggleToFight;
    }

    public void ToggleToLanding(LandingData landingData)
    {
        fightButtons.gameObject.SetActive(false);
        prepareButtons.gameObject.SetActive(true);
    }


    public void ToggleToFight()
    {
        fightButtons.gameObject.SetActive(true);
        prepareButtons.gameObject.SetActive(false);
    }

    public void OnPressToBattleButton()
    {
        _behaviour.OnPressToBattleButton(this);
    }

    public void OnPressToBaseButton()
    {
        _behaviour.OnPressToBaseButton(this);
    }

    public void OnPressCleanLandingButton()
    {
        _behaviour.OnPressCleanLandingButton(this);
    }

    public void OnPressChangeBaseButton()
    {
        _behaviour.OnPressChangeBaseButton(this);
    }

    public void OnPressSupportButton()
    {
        _behaviour.OnPressSupportButton(this);
    }

    public void OnPressSurrenderButton()
    {
        _behaviour.OnPressSurrenderButton(this);
    }

    public void OnPressPassButton()
    {
        _behaviour.OnPressPassButton(this);
    }

    public void OnSomeComponentChangeBehaviour(string componentName, string behaviourName)
    {
        if (componentName != Type) return;
        ChangeBehaviour(behaviourName);
    }

    public void OnResetBehaviour()
    {
        ChangeBehaviour();
    }

    public void ChangeBehaviour(string name = "Base")
    {
        _behaviour = SubsituableFightOptionsFactory.GetBehaviourById(name);
        _behaviour.Init(this);
    }
}
