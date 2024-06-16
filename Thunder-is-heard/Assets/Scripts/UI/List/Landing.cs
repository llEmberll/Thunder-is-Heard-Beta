

using System.Collections.Generic;
using UnityEngine;

public class Landing : ItemList
{
    public Dictionary<string, UnitInventoryItem> items;

    public override void FillContent()
    {
    }

    private bool IsItemExist(string id)
    {
        return items.ContainsKey(id);
    }

    private void IncrementCountOfItem(string itemId)
    {
        items[itemId].Increment();
    }

    private void CreateAndAddItem(string id, string name, string pathToIcon)
    {
        Sprite icon = Resources.Load<Sprite>(pathToIcon);

        GameObject unitItemPrefab = Resources.Load<GameObject>(Config.resources["landableUnitItem"]);
        GameObject unitItemObj = Instantiate(unitItemPrefab, unitItemPrefab.transform.position, Quaternion.identity);
        //ExposedItem unitItem = unitItemObj.GetComponent<ExposedItem>();
        //unitItem.Init(id, name, itemType, 1, icon);

        //TODO Создать префаб-карточку для LandableUnit по аналогии с InventoryItem,  Реализовать под LandableUnit (UI Item)

        //items.Add(id, unitItem);
    }
}
