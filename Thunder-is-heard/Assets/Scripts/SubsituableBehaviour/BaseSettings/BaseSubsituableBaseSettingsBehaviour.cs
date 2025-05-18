
public class BaseSubsituableBaseSettingsBehaviour : ISubsituableBaseSettingsBehaviour
{
    public virtual void Init(BaseSettingsPanel conductor)
    {
        conductor.TurnOnRenameOption();
    }

    public virtual void OnRenameOption(BaseSettingsPanel conductor)
    {
        conductor.renameBaseModal.Toggle();
    }
}
