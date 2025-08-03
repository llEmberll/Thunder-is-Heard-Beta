using UnityEngine;

public class DisabledLandingBehaviour : BaseSubsituableLandingBehaviour
{
    public override void Init(Landing conductor)
    {
        conductor.Hide();
    }

    public override void OnLandableUnitFocus(Landing conductor, LandableUnit target)
    {
    }

    public override void OnLandableUnitDefocus(Landing conductor, LandableUnit target)
    {
    }



    public override void OnObjectLanded(Landing conductor, Entity entity)
    {
    }

    public override void OnObjectRemoved(Landing conductor, Entity entity)
    {
    }

    public override void OnPressedToBattleButton(Landing conductor)
    {
    }

    public override void StartLanding(Landing conductor, LandingData landingData)
    {
        Debug.Log("StartLanding from DISABLED BEHAVIOUR");

        conductor.Show();
        conductor.ChangeBehaviour();
        conductor.StartLanding(landingData);
    }

    public override void FinishLanding(Landing conductor)
    {
    }

    public override void CreatePreview(Landing conductor, ExposableInventoryItem item)
    {
    }

    public override void OnObjectExposed(Landing conductor, ExposableInventoryItem item, Entity obj)
    {
    }

    public override void OnInventoryItemAdded(Landing conductor, InventoryItem sourceItem, InventoryCacheItem addedItem)
    {
    }

    public override void Substract(Landing conductor, InventoryItem item, int number = 1)
    {
    }

    public override void Increment(Landing conductor, InventoryItem item, int number = 1)
    {
    }
}
