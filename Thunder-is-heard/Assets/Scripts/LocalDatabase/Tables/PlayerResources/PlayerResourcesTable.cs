using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "player resource table", fileName = "player resources")]
public class PlayerResourcesTable : SomeTable
{
    public new List<PlayerResourceData> items;
    public new PlayerResourceData currentItem;

    public new List<PlayerResourceData> Items { get { return items; } set { } }

    public override string Name
    {
        get
        {
            return "PlayerResource";
        }
    }

    public new void AddElement()
    {
        if (items == null)
        {
            items = new List<PlayerResourceData>();
        }

        if (currentItem == null)
        {
            currentItem = new PlayerResourceData();
        }

        currentItem = (PlayerResourceData)currentItem.Clone();

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

    public new PlayerResourceData GetNext()
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

    public new PlayerResourceData GetPrev()
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

    public new PlayerResourceData this[int index]
    {
        get
        {
            if (items != null && index >= 0 && index < items.Count)
            {
                return items[index];
            }
            return default(PlayerResourceData);
        }
        set
        {
            if (items == null)
            {
                items = new List<PlayerResourceData>();
            }

            if (index >= 0 && index < items.Count && value != null)
            {
                items[index] = value;
            }
            else
            {
                Debug.Log("����� �� ������� ������� ��� �������� ������� ��������");
            }
        }
    }

    public override ITableItem DefaultItem()
    {
        return new PlayerResourceData();
    }
}