
public class EditLandedPanel : Panel
{
    public void Start()
    {
        InitListeners();
        Hide();
    }

    public void InitListeners()
    {
        EnableListeners();
    }

    public void EnableListeners()
    {
        EventMaster.current.StartLanding += OnStartLanding;
        EventMaster.current.FightIsStarted += OnFinishLanding;
        EventMaster.current.FightIsContinued += OnFinishLanding;
        EventMaster.current.ToggledToBuildMode += Hide;
        EventMaster.current.ToggledOffBuildMode += Show;
    }

    public void DisableListeners()
    {
        EventMaster.current.ToggledToBuildMode -= Hide;
        EventMaster.current.ToggledOffBuildMode -= Show;
        EventMaster.current.FightIsStarted -= OnFinishLanding;
        EventMaster.current.FightIsContinued -= OnFinishLanding;
    }

    public void OnStartLanding(LandingData landingData)
    {
        Show();
    }

    public void OnFinishLanding()
    {
        DisableListeners();
        Hide();
    }
}
