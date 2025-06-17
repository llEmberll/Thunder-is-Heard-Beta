using System;
using System.Collections.Generic;

public static class SubsituableCellFactory
{
    public static Dictionary<string, Type> behaviours = new Dictionary<string, Type>()
    {
        { "Base", typeof(BaseSubsituableCellBehaviour) },
        { "Disabled", typeof(DisabledSubsituableCellBehaviour) },
    };

    public static ISubsituableCellBehaviour GetBehaviourById(string id)
    {
        if (behaviours.ContainsKey(id))
        {
            Type type = behaviours[id];
            return (ISubsituableCellBehaviour)Activator.CreateInstance(type);
        }

        return null;
    }
}
