using System.Collections.Generic;

public interface ISubsituableContractsBehaviour
{
    public void Init(Contracts conductor);

    public List<ContractItem> GetItems(Contracts conductor);

    public bool IsAvailableToBuy(ContractItem item);
    public void OnBuy(ContractItem item);

    public void Toggle(Contracts conductor);

    public void FillContent(Contracts conductor);


    public void OnInteractWithIdleComponent(ContractComponent component);
    public void OnInteractWithWorkingComponent(ContractComponent component);
    public void OnInteractWithFinishedComponent(ContractComponent component);
}
