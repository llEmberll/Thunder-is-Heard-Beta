using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "inventory table", fileName = "inventory")]
public class InventoryTable : SomeTable
{
    public new List<InventoryData> items;
    public new InventoryData currentItem;

    public new List<InventoryData> Items { get { return items; } set { } }

    public override string Name
    {
        get
        {
            return "Inventory";
        }
    }

    public new void AddElement()
    {
        if (items == null)
        {
            items = new List<InventoryData>();
        }

        if (currentItem == null)
        {
            currentItem = new InventoryData();
        }

        currentItem = (InventoryData)currentItem.Clone();

        items.Add(currentItem);
        currentIndex = items.Count - 1;
    }

    public new void RemoveElement()
    {
        if (currentIndex > 0)
        {
            currentItem = items[currentIndex - 1];
            items.RemoveAt(currentIndex);
            currentIndex--;
        }

        else
        {
            if (items.Count > currentIndex)
            {
                items.RemoveAt(currentIndex);
                currentItem = null;
            }
        }
    }

    public new InventoryData GetNext()
    {
        if (items.Count == 0)
        {
            return currentItem;
        }

        if (currentIndex < items.Count - 1)
        {
            currentIndex++;
        }

        else
        {
            currentIndex = 0;
        }

        currentItem = items[currentIndex];
        return currentItem;
    }

    public new InventoryData GetPrev()
    {
        if (currentIndex == 0)
        {
            if (items.Count == 0)
            {
                return currentItem;
            }

            currentIndex = items.Count - 1;
            return currentItem = items[currentIndex];
        }

        else
        {
            currentIndex--;
            currentItem = items[currentIndex];
            return currentItem;
        }
    }

    public new void ClearAll()
    {
        items.Clear();
        currentIndex = 0;
    }

    public new InventoryData this[int index]
    {
        get
        {
            if (items != null && index >= 0 && index < items.Count)
            {
                return items[index];
            }
            return default(InventoryData);
        }
        set
        {
            if (items == null)
            {
                items = new List<InventoryData>();
            }

            if (index >= 0 && index < items.Count && value != null)
            {
                items[index] = value;
            }
            else
            {
                Debug.Log("Выход за границы массива или передано нулевое значение");
            }
        }
    }

    public override ITableItem DefaultItem()
    {
        return new InventoryData();
    }
}
