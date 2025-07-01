using System;
using System.Collections.Generic;

public class AIFactory
{
    public static Dictionary<string, Type> AITypes = new Dictionary<string, Type>()
    {
        { "Waiting", typeof(WaitingAI) },
        { "Attacking", typeof(AttackingAI) },
        { "Frozen", typeof(FrozenAI) },
        { "Pursuing", typeof (PursuingAI) },
    };

    public static AIInterface GetConfiguredAIByTypeAndSettings(AISettings settings)
    {
        if (!AITypes.ContainsKey(settings.type)) return null;

        Type AIType = AITypes[settings.type];
        AbstractAI abstractAI = (AbstractAI)Activator.CreateInstance(AIType);
        abstractAI.settings = settings;
        abstractAI.Init();
        return abstractAI;
    }
}
