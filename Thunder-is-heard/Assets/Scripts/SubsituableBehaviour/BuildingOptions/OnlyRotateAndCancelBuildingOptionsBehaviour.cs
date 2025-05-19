
public class OnlyRotateAndCancelBuildingOptionsBehaviour : BaseSubsituableBuildingOptionsBehaviour
{

    public override void OnBuildMode(BuildingPanel conductor)
    {
        conductor.turnOnBuildingOption.gameObject.SetActive(false);
        conductor.turnOffBuildingOption.gameObject.SetActive(false);
        conductor.toInventoryOption.gameObject.SetActive(false);
        conductor.sellOption.gameObject.SetActive(false);

        conductor.rotateOption.gameObject.SetActive(true);
        conductor.cancelOption.gameObject.SetActive(true);
    }

    public override void OnExitBuildMode(BuildingPanel conductor)
    {
        conductor.TurnOffAllOptions();
    }

    public override void TurnOnBuilding(BuildingPanel conductor)
    {
    }

    public override void ToInventory(BuildingPanel conductor)
    {
    }

    public override void Sell(BuildingPanel conductor)
    {
    }
}
