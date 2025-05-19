
public class DisabledBuildingOptionsBehaviour : BaseSubsituableBuildingOptionsBehaviour
{
    public override void Init(BuildingPanel conductor)
    {
        conductor.TurnOffAllOptions();   
    }

    public override void OnBuildMode(BuildingPanel conductor)
    {
    }

    public override void OnExitBuildMode(BuildingPanel conductor)
    {
    }

    public override void TurnOffBuilding(BuildingPanel conductor)
    {
    }

    public override void TurnOnBuilding(BuildingPanel conductor)
    {
    }

    public override void Cancel(BuildingPanel conductor)
    {
    }

    public override void Rotate(BuildingPanel conductor)
    {
    }

    public override void ToInventory(BuildingPanel conductor)
    {
    }

    public override void Sell(BuildingPanel conductor)
    {
    }
}
