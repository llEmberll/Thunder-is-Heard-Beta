using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "player build table", fileName = "player builds")]
public class PlayerBuildsTable : SomeTable
{
    public List<BuildData> items;
    public BuildData currentItem;

    public new List<BuildData> Items { get { return items; } set { } }

    public override string Name
    {
        get
        {
            return "PlayerBuild";
        }
    }

    public void AddElement()
    {
        if (items == null)
        {
            items = new List<BuildData>();
        }

        currentItem = new BuildData();
        items.Add(currentItem);
        currentIndex = items.Count - 1;
    }

    public void RemoveElement()
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

    public BuildData GetNext()
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

    public BuildData GetPrev()
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

    public void ClearAll()
    {
        items.Clear();
        currentIndex = 0;
    }

    public BuildData this[int index]
    {
        get
        {
            if (items != null && index >= 0 && index < items.Count)
            {
                return items[index];
            }
            return default(BuildData);
        }
        set
        {
            if (items == null)
            {
                items = new List<BuildData>();
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