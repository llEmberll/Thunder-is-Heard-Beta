using System;
using System.Linq;

public class OnlyTutorialProductionsUnitProductionsBehaviour : BaseSubsituableUnitProductionsBehaviour
{
    public override bool CheckUnitProductionCircumstancesRequirements(string[] circumstances)
    {
        return circumstances.Contains("tutorial");
    }
}
