using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Table", fileName = "table")]
public class SomeTable : ScriptableObject, ITable
{
    public List<SomeTableItem> items = new List<SomeTableItem>();
    public SomeTableItem currentItem;

    public int currentIndex = 0;

    public virtual string Name { get { return "Undefined"; } set { } }

    public virtual List<SomeTableItem> Items { get { return items; } set { } }

    public void AddElement()
    {
        if (items == null)
        {
            DefaultItemList();
        }

        currentItem = DefaultItem();
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

    public void GetNext()
    {
        if (items.Count == 0)
        {
            return;
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
        return;
    }

    public void GetPrev()
    {
        if (currentIndex == 0)
        {
            if (items.Count == 0)
            {
                return;
            }

            currentIndex = items.Count - 1;
            currentItem = items[currentIndex];
            return;
        }

        else
        {
            currentIndex--;
            currentItem = items[currentIndex];
            return;
        }
    }

    public void ClearAll()
    {
        items.Clear();
        currentIndex = 0;
    }

    public SomeTableItem this[int index]
    {
        get
        {
            if (items != null && index >= 0 && index < items.Count)
            {
                return items[index];
            }
            return DefaultItem();
        }
        set
        {
            if (items == null)
            {
                DefaultItemList();
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

    public virtual void DefaultItemList()
    {
        items = new List<SomeTableItem>();
    }

    public virtual SomeTableItem DefaultItem()
    {
        return new SomeTableItem();
    }
}
