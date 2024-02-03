

using System.Collections.Generic;
using UnityEngine;

public class Landing : ItemList
{
    public string itemType = "PlayerUnit";

    public Dictionary<int, ExposedItem> items;

    public PlayerUnitsTable playerUnitsTable;

    public override void FillContent()
    {
        playerUnitsTable = (PlayerUnitsTable)LocalDatabase.GetTableByName(itemType);

        foreach (UnitData unitData in playerUnitsTable.Items)
        {
            Dictionary<string, object> unitFields = unitData.GetFields();
            int id = (int)unitFields["externalId"];

            if (IsItemExist(id))
            {
                IncrementCountOfItem(id);
            }

            else
            {
                CreateAndAddItem(id, (string)unitFields["name"], (string)unitFields["icon"]);
            }
        }
    }

    private bool IsItemExist(int id)
    {
        return items.ContainsKey(id);
    }

    private void IncrementCountOfItem(int itemId)
    {
        items[itemId].Increment();
    }

    private void CreateAndAddItem(int id, string name, string pathToIcon)
    {
        Sprite icon = Resources.Load<Sprite>(pathToIcon);

        GameObject unitItemPrefab = Resources.Load<GameObject>(Config.resources["landableUnitItem"]);
        GameObject unitItemObj = Instantiate(unitItemPrefab, unitItemPrefab.transform.position, Quaternion.identity);
        ExposedItem unitItem = unitItemObj.GetComponent<ExposedItem>();
        unitItem.Init(id, name, itemType, icon);

        items.Add(id, unitItem);
    }
}
