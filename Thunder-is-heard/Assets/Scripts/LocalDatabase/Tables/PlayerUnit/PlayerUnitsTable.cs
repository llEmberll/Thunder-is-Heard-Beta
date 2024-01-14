using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "player unit table", fileName = "player units")]
[System.Serializable]
public class PlayerUnitsTable : SomeTable
{
    public new List<UnitData> items;
    public new UnitData currentItem;

    public new List<UnitData> Items { get { return items; } set { } }

    public override string Name
    {
        get
        {
            return "PlayerUnit";
        }
    }

    public new void AddElement()
    {
        if (items == null)
        {
            items = new List<UnitData>();
        }

        currentItem = new UnitData();
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

    public new UnitData GetNext()
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

    public new UnitData GetPrev()
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

    public new UnitData this[int index]
    {
        get
        {
            if (items != null && index >= 0 && index < items.Count)
            {
                return items[index];
            }
            return default(UnitData);
        }
        set
        {
            if (items == null)
            {
                items = new List<UnitData>();
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
}