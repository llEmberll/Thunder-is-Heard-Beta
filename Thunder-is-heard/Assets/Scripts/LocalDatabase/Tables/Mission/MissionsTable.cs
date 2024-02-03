using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "missions table", fileName = "missions")]
public class MissionsTable : SomeTable
{
    public new List<MissionData> items;
    public new MissionData currentItem;

    public new List<MissionData> Items { get { return items; } set { } }

    public override string Name
    {
        get
        {
            return "Mission";
        }
    }

    public new void AddElement()
    {
        if (items == null)
        {
            items = new List<MissionData>();
        }

        if (currentItem == null)
        {
            currentItem = new MissionData();
        }

        currentItem = (MissionData)currentItem.Clone();

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

    public new MissionData GetNext()
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

    public new MissionData GetPrev()
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

    public new MissionData this[int index]
    {
        get
        {
            if (items != null && index >= 0 && index < items.Count)
            {
                return items[index];
            }
            return default(MissionData);
        }
        set
        {
            if (items == null)
            {
                items = new List<MissionData>();
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
        return new MissionData();
    }
}
