

public class LandingBuildingOptionsBehaviour : BaseSubsituableBuildingOptionsBehaviour
{
    // в разработке

    public override void OnBuildMode(BuildingPanel conductor)
    {
        // включить Cancel, ToInv и Rotate, остальное выкл.
    }

    public override void OnExitBuildMode(BuildingPanel conductor)
    {
        // включить editLanding, остальное выкл.
    }

    public override void ToInventory(BuildingPanel conductor)
    {
        // вернуть объект в высаживаемых юнитов
    }
}
