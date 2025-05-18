using UnityEngine.UI;


public class BaseSettingsPanel : Panel
{
    public ChangeBaseNameModal renameBaseModal;

    public Button renameBaseOptionButton;


    public static string ComponentType
    {
        get { return "BaseSettingsPanel"; }
    }

    public ISubsituableBaseSettingsBehaviour _behaviour;


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
        EventMaster.current.ComponentBehaviourChanged += OnSomeComponentChangeBehaviour;
    }

    public virtual void DisableListeners()
    {
        EventMaster.current.ComponentBehaviourChanged -= OnSomeComponentChangeBehaviour;
    }

    public virtual void OnRenameOption()
    {
        _behaviour.OnRenameOption(this); 

    }

    public virtual void TurnOffRenameOption()
    {
        renameBaseOptionButton.interactable = false;
    }

    public virtual void TurnOnRenameOption()
    {
        renameBaseOptionButton.interactable = true;
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
        _behaviour = SubsituableBaseSettingsFactory.GetBehaviourById(name);
        _behaviour.Init(this);
    }
}
