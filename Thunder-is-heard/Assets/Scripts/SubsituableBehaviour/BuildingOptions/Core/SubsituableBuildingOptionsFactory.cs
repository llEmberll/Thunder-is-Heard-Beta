using System;
using System.Collections.Generic;

public static class SubsituableBuildingOptionsFactory
{
    public static Dictionary<string, Type> behaviours = new Dictionary<string, Type>()
    {
        { "Base", typeof(BaseSubsituableBuildingOptionsBehaviour) },
        { "OnlyRotateAndCancel", typeof(OnlyRotateAndCancelBuildingOptionsBehaviour) },
        { "Disabled", typeof(DisabledBuildingOptionsBehaviour) },
    };

    public static ISubsituableBuildingOptionsBehaviour GetBehaviourById(string id)
    {
        if (behaviours.ContainsKey(id))
        {
            Type type = behaviours[id];
            return (ISubsituableBuildingOptionsBehaviour)Activator.CreateInstance(type);
        }

        return null;
    }
}
