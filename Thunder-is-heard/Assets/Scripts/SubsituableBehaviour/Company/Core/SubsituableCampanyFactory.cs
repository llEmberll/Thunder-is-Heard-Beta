using System;
using System.Collections.Generic;

public static class SubsituableCampanyFactory
{
    public static Dictionary<string, Type> behaviours = new Dictionary<string, Type>()
    {
        { "Base", typeof(BaseSubsituableCampanyBehaviour) },
    };

    public static ISubsituableCampanyBehaviour GetBehaviourById(string id)
    {
        if (behaviours.ContainsKey(id))
        {
            Type type = behaviours[id];
            return (ISubsituableCampanyBehaviour)Activator.CreateInstance(type);
        }

        return null;
    }
}
