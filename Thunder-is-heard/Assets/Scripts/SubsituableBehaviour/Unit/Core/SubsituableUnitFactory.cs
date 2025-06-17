using System;
using System.Collections.Generic;

public static class SubsituableUnitFactory
{
    public static Dictionary<string, Type> behaviours = new Dictionary<string, Type>()
    {
        { "Base", typeof(BaseSubsituableUnitBehaviour) },
        { "Disabled", typeof(DisabledSubsituableUnitBehaviour) },
    };

    public static ISubsituableUnitBehaviour GetBehaviourById(string id)
    {
        if (behaviours.ContainsKey(id))
        {
            Type type = behaviours[id];
            return (ISubsituableUnitBehaviour)Activator.CreateInstance(type);
        }

        return null;
    }
}
