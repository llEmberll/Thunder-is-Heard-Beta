using Newtonsoft.Json;
using System.Collections.Generic;


[System.Serializable]
public class FocusData
{
    [JsonProperty("Type")]
    public string Type;

    [JsonProperty("Data")]
    public Dictionary<string, object> Data;


    // Example 1. ќпциональна€ кнопка по тегу
    /// <summary>
    /// {"Type": "Button", "Data": {"tag": "ToShopButton", ...}}
    /// </summary>

    // Example 2. »гровой объект по дочернему id
    /// <summary>
    /// {"Type": "Build", "Data": {"childId": "[some uuid4]", ...}}
    /// </summary>

    // Example 3. ѕервый игровой объект по родительскому id
    /// <summary>
    /// {"Type": "Build", "Data": {"coreId": "[some uuid4]", ...}}
    /// </summary>

    // Example 4. Ёлемент списка по типу списка и id элемента
    /// <summary>
    /// {"Type": "UIItem", "Data": {"UIType": "Shop", "itemId": "some uuid4", ...}}
    /// </summary>

    // Example 5. Ёлемент списка по типу списка и родительскому id элемента
    /// <summary>
    /// {"Type": "UIItem", "Data": {"UIType": "Shop", "coreId": "some uuid4", ...}}
    /// </summary>

    // Example 6. Ёлемент списка по типу списка и родительскому id элемента
    /// <summary>
    /// {"Type": "UIItem", "Data": {"UIType": "Contracts", "contractType": "Steel", ...}}
    /// </summary>

    // Example 7. Ёлемент списка по типу списка и родительскому id элемента
    /// <summary>
    /// {"Type": "UIItem", "Data": {"UIType": "UnitProductions", "itemId": "some uuid4", ...}}
    /// </summary>

    // Example 8. Ёлемент списка по типу списка и родительскому id элемента
    /// <summary>
    /// {"Type": "UIItem", "Data": {"UIType": "UnitProductions", "unitType": "Infantry", ...}}
    /// </summary>

    // Example 9. Ёлемент списка по типу списка и родительскому id элемента
    /// <summary>
    /// {"Type": "UIItem", "Data": {"UIType": "UnitProductions", "unitId": "some uuid4", ...}}
    /// </summary>

    // Example 10. ѕервое уведомление о сборе по типу уведомлени€
    /// <summary>
    /// {"Type": "ProductsNotification", "Data": {"type": "ResourceCollection", ...}}
    /// </summary>

    // Example 11. ѕервое уведомление о сборе по родительскому id объекта-источника
    /// <summary>
    /// {"Type": "ProductsNotification", "Data": {"sourceObjectCoreId": "8878498b-a4bc-4dc8-8f39-bc9e987a689f", ...}}
    /// </summary>


    public FocusData() { }

    public FocusData(string type, Dictionary<string, object> data)
    {
        Type = type;
        Data = data;
    }
}
