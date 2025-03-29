using System.Collections.Generic;

public interface ISubsituableContractsBehaviour
{
    public void Init();

    public List<ContractItem> GetItems(Contracts conductor);

    public bool IsAvailableToBuy(ContractItem item);
    public void OnBuy(ContractItem item);

}
