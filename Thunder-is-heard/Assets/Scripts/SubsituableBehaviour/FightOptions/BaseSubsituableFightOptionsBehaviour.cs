using UnityEngine;


public class BaseSubsituableFightOptionsBehaviour : ISubsituableFightOptionsBehaviour
{
    public virtual void Init(FightPanel conductor)
    {

    }

    public virtual void OnPressToBattleButton(FightPanel conductor)
    {
        EventMaster.current.StartFight();
    }

    public virtual void OnPressToBaseButton(FightPanel conductor)
    {
        FightDirector.ReturnToBase();
    }

    public virtual void OnPressCleanLandingButton(FightPanel conductor)
    {

    }

    public virtual void OnPressChangeBaseButton(FightPanel conductor)
    {

    }

    public virtual void OnPressSupportButton(FightPanel conductor)
    {

    }

    public virtual void OnPressSurrenderButton(FightPanel conductor)
    {

    }

    public virtual void OnPressPassButton(FightPanel conductor)
    {
        Debug.Log("Base fight panel behaviour: press on PASS");

        EventMaster.current.OnPassTurn();
    }
}
