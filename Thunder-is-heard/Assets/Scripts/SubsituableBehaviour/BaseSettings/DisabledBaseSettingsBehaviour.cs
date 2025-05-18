
public class DisabledBaseSettingsBehaviour : BaseSubsituableBaseSettingsBehaviour
{
    public override void Init(BaseSettingsPanel conductor)
    {
        conductor.TurnOffRenameOption();
    }

    public override void OnRenameOption(BaseSettingsPanel conductor)
    {
    }
}
