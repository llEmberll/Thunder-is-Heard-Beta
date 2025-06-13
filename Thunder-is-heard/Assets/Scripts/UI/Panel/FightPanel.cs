using UnityEngine.UI;

public class FightPanel : Panel
{
    public Image fightButtons;
    public Image prepareButtons;

    public void Awake()
    {
        EnableListeners();
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
}
