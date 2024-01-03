using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "base cells table", fileName = "base cells")]
public class BaseCellsTable : SomeTable
{
    public List<BaseCellsData> items;
    public BaseCellsData currentItem;

    public new List<BaseCellsData> Items { get { return items; } set { } }

    public override string Name
    {
        get
        {
            return "PlayerCell";
        }
    }

    public void AddElement()
    {
        if (items == null)
        {
            items = new List<BaseCellsData>();
        }

        currentItem = new BaseCellsData();
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

    public BaseCellsData GetNext()
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

    public BaseCellsData GetPrev()
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

    public BaseCellsData this[int index]
    {
        get
        {
            if (items != null && index >= 0 && index < items.Count)
            {
                return items[index];
            }
            return default(BaseCellsData);
        }
        set
        {
            if (items == null)
            {
                items = new List<BaseCellsData>();
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

    public void GenerateDefaultCells(int size)
    {
        Debug.Log("size: " + size);

        this.items = new List<BaseCellsData>();
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                BaseCellsData data = new BaseCellsData();
                data.position = new Vector2Int(x, y);

                this.items.Add(data);
            }
        }
    }
}