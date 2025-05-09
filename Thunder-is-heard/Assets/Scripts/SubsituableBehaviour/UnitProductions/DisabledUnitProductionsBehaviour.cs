using UnityEngine;

public class DisabledUnitProductionsBehaviour : BaseSubsituableUnitProductionsBehaviour
{


    public override void Init(UnitProductions conductor)
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag(Tags.resourcesProcessor).GetComponent<ResourcesProcessor>();
    }

    public override void OnBuy(UnitProductionItem item)
    {

    }

    public override void Toggle(UnitProductions conductor)
    {

    }


    public override void OnInteractWithIdleComponent(UnitProductionComponent component)
    {

    }

    public override void OnInteractWithWorkingComponent(UnitProductionComponent component)
    {

    }

    public override void OnInteractWithFinishedComponent(UnitProductionComponent component)
    {

    }
}
