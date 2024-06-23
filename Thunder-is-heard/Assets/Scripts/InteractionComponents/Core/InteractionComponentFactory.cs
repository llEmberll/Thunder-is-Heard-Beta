using System;
using System.Collections.Generic;

public static class InteractionComponentFactory
{
    public static Dictionary<string, Type> components = new Dictionary<string, Type>()
    {
        { "Inaction", typeof(Inaction) },
        { "ContractComponent", typeof(ContractComponent) },
        { "UnitProductionComponent", typeof(UnitProductionComponent) },
        { "ImprovementComponent", typeof(ImprovementComponent) },
    };

    public static InteractionComponent GetComponentById(string id)
    {
        if (components.ContainsKey(id))
        {
            Type type = components[id];
            return (InteractionComponent)Activator.CreateInstance(type);
        }

        return null;
    }
}
