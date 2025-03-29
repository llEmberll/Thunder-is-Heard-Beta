using System;
using System.Collections.Generic;

public static class SubsituableUnitProductionsFactory
{
    public static Dictionary<string, Type> behaviours = new Dictionary<string, Type>()
    {
        { "Base", typeof(BaseSubsituableUnitProductionsBehaviour) },
    };

    public static ISubsituableUnitProductionsBehaviour GetBehaviourById(string id)
    {
        if (behaviours.ContainsKey(id))
        {
            Type type = behaviours[id];
            return (ISubsituableUnitProductionsBehaviour)Activator.CreateInstance(type);
        }

        return null;
    }
}
