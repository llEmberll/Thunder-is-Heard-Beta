using System;
using System.Collections.Generic;

public static class SubsituableObstacleFactory
{
    public static Dictionary<string, Type> behaviours = new Dictionary<string, Type>()
    {
        { "Base", typeof(BaseSubsituableObstacleBehaviour) },
        { "Disabled", typeof(DisabledObstacleBehaviour) },
    };

    public static ISubsituableObstacleBehaviour GetBehaviourById(string id)
    {
        if (behaviours.ContainsKey(id))
        {
            Type type = behaviours[id];
            return (ISubsituableObstacleBehaviour)Activator.CreateInstance(type);
        }

        return null;
    }
}
