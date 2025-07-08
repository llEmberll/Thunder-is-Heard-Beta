using System;
using System.Collections.Generic;

public static class SubsituableFightOptionsFactory
{
    public static Dictionary<string, Type> behaviours = new Dictionary<string, Type>()
    {
        { "Base", typeof(BaseSubsituableFightOptionsBehaviour) },
        { "NoBaseReturn", typeof(FightOptionsWithoutReturnToBaseBehaviour) },
        { "Disabled", typeof(DisabledFightOptionsBehaviour) },
        { "OnlyPass", typeof(FightOptionsWithOnlyPassBehaviour) }
    };

    public static ISubsituableFightOptionsBehaviour GetBehaviourById(string id)
    {
        if (behaviours.ContainsKey(id))
        {
            Type type = behaviours[id];
            return (ISubsituableFightOptionsBehaviour)Activator.CreateInstance(type);
        }

        return null;
    }
}
