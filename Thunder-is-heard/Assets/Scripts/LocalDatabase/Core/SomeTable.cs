using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Table", fileName = "table")]
public abstract class SomeTable : ScriptableObject, ITable
{
    public List<ITableItem> items = new List<ITableItem>();
    public ITableItem currentItem;

    public int currentIndex = 0;

    public virtual string Name { get { return "Undefined"; } set { } }

    public virtual List<ITableItem> Items { get { return items; } set { } }

    public virtual void AddElement()
    {
        if (items == null)
        {
            DefaultItemList();
        }

        currentItem = DefaultItem();
        items.Add(currentItem);
        currentIndex = items.Count - 1;
    }
    public virtual void RemoveElement()
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

    public virtual void GetNext()
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

    public virtual void GetPrev()
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

    public virtual void ClearAll()
    {
        items.Clear();
        currentIndex = 0;
    }

    public virtual ITableItem this[int index]
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
                Debug.Log("����� �� ������� ������� ��� �������� ������� ��������");
            }
        }
    }

    public virtual void DefaultItemList()
    {
        items = new List<ITableItem>();
    }

    public abstract ITableItem DefaultItem();
}