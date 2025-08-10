using UnityEngine;


public class FightOptionsWithoutReturnToBaseBehaviour : BaseSubsituableFightOptionsBehaviour
{


    public override void OnPressToBaseButton(FightPanel conductor)
    {
        
    }

    public override void OnPressPassButton(FightPanel conductor)
    {
        Debug.Log("No return base fight panel behaviour: press on PASS");

        base.OnPressPassButton(conductor);
    }
}
