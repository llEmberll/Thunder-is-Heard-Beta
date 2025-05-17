using System;
using System.Linq;

public class OnlyTutorialContractContractsBehaviour : BaseSubsituableContractsBehaviour
{
    public override bool CheckContractCircumstancesRequirements(string[] circumstances)
    {
        return circumstances.Contains("tutorial");
    }
}
