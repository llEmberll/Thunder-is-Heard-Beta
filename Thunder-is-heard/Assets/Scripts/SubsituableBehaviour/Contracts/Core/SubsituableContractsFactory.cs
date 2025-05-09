using System;
using System.Collections.Generic;

public static class SubsituableContractsFactory
{
    public static Dictionary<string, Type> behaviours = new Dictionary<string, Type>()
    {
        { "Base", typeof(BaseSubsituableContractsBehaviour) },
        { "OnlyTutorialContracts", typeof (OnlyTutorialContractContractsBehaviour) },
        { "Disabled", typeof (DisabledContractsBehaviour) },
    };

    public static ISubsituableContractsBehaviour GetBehaviourById(string id)
    {
        if (behaviours.ContainsKey(id))
        {
            Type type = behaviours[id];
            return (ISubsituableContractsBehaviour)Activator.CreateInstance(type);
        }

        return null;
    }
}
