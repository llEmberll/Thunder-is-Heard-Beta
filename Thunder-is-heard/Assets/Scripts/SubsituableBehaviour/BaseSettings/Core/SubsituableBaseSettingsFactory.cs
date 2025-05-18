using System;
using System.Collections.Generic;

public static class SubsituableBaseSettingsFactory
{
    public static Dictionary<string, Type> behaviours = new Dictionary<string, Type>()
    {
        { "Base", typeof(BaseSubsituableBaseSettingsBehaviour) },
        { "Disabled", typeof(DisabledBaseSettingsBehaviour) },
    };

    public static ISubsituableBaseSettingsBehaviour GetBehaviourById(string id)
    {
        if (behaviours.ContainsKey(id))
        {
            Type type = behaviours[id];
            return (ISubsituableBaseSettingsBehaviour)Activator.CreateInstance(type);
        }

        return null;
    }
}
