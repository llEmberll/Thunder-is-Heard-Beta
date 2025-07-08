using System;
using System.Collections.Generic;

public static class SubsituableLandingFactory
{
    public static Dictionary<string, Type> behaviours = new Dictionary<string, Type>()
    {
        { "Base", typeof(BaseSubsituableLandingBehaviour) },
        { "Disabled", typeof(DisabledLandingBehaviour) },
        { "WithFiveAssaulters", typeof(LandingWithFiveAssaultersBehaviour) }
    };

    public static ISubsituableLandingBehaviour GetBehaviourById(string id)
    {
        if (behaviours.ContainsKey(id))
        {
            Type type = behaviours[id];
            return (ISubsituableLandingBehaviour)Activator.CreateInstance(type);
        }

        return null;
    }
}
