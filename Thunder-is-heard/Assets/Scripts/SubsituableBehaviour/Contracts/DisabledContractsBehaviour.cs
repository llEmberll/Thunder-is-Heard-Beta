using UnityEngine;

public class DisabledContractsBehaviour : BaseSubsituableContractsBehaviour
{

    public override void Init(Contracts conductor)
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag(Tags.resourcesProcessor).GetComponent<ResourcesProcessor>();
    }

    public override void OnBuy(ContractItem item)
    {

    }

    public override void Toggle(Contracts conductor)
    {

    }

    public override void OnInteractWithIdleComponent(ContractComponent component)
    {

    }

    public override void OnInteractWithWorkingComponent(ContractComponent component)
    {

    }

    public override void OnInteractWithFinishedComponent(ContractComponent component)
    {

    }
}
