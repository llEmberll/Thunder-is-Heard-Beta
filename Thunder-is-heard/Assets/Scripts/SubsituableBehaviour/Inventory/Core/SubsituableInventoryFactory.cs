using System;
using System.Collections.Generic;

public static class SubsituableInventoryFactory
{
    public static Dictionary<string, Type> behaviours = new Dictionary<string, Type>()
    {
        { "Base", typeof(BaseSubsituableInventoryBehaviour) },
        { "Disabled", typeof(DisabledInventoryBehaviour) },
    };

    public static ISubsituableInventoryBehaviour GetBehaviourById(string id)
    {
        if (behaviours.ContainsKey(id))
        {
            Type type = behaviours[id];
            return (ISubsituableInventoryBehaviour)Activator.CreateInstance(type);
        }

        return null;
    }
}
