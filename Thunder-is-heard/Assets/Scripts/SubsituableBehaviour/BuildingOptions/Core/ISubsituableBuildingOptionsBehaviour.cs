

public interface ISubsituableBuildingOptionsBehaviour
{
    public void Init(BuildingPanel conductor);

    public void OnBuildMode(BuildingPanel conductor);

    public void OnExitBuildMode(BuildingPanel conductor);

    public void TurnOnBuilding(BuildingPanel conductor);

    public void TurnOffBuilding(BuildingPanel conductor);

    public void Cancel(BuildingPanel conductor);

    public void Rotate(BuildingPanel conductor);

    public void ToInventory(BuildingPanel conductor);

    public void Sell(BuildingPanel conductor);


}
