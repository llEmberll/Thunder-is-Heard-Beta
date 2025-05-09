using System;
using System.Collections.Generic;

public static class SubsituableShopFactory
{
    public static Dictionary<string, Type> behaviours = new Dictionary<string, Type>()
    {
        { "Base", typeof(BaseSubsituableShopBehaviour) },
        { "Disabled", typeof(DisabledShopBehaviour) },
        { "OnlyOffice", typeof (OnlyOfficeShopBehaviour) },
        { "OnlyOilStation", typeof (OnlyOilStationShopBehaviour) },
        { "OnlyTrainingCenter", typeof (OnlyTrainingCenterShopBehaviour) },
        { "OnlyTent", typeof (OnlyTentShopBehaviour) },
        { "OnlyWarehouse", typeof (OnlyWarehouseShopBehaviour) },
        { "OnlyLaboratory", typeof(OnlyLaboratoryShopBehaviour) },
    };

    public static ISubsituableShopBehaviour GetBehaviourById(string id)
    {
        if (behaviours.ContainsKey(id))
        {
            Type type = behaviours[id];
            return (ISubsituableShopBehaviour)Activator.CreateInstance(type);
        }

        return null;
    }
}
