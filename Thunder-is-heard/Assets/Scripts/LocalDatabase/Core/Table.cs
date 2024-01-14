
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Table", fileName = "table")]
public class Table<T>:ScriptableObject where T : TableItem, new()
{
    public new virtual string name { get; set; }

    public List<T> items;
    public T currentItem;

    public int currentIndex = 0;

    public void AddElement()
    {
        if (items == null)
        {
            items = new List<T>();
        }

        currentItem = new T();
        items.Add(currentItem);
        currentIndex = items.Count - 1;
        currentItem.ExternalId = currentIndex;
    }
    public void RemoveElement()
    {
        if (currentIndex > 0)
        {
            currentItem = items[--currentIndex];
            items.RemoveAt(++currentIndex);
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

    public T GetNext()
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

    public T GetPrev()
    {
        if ( currentIndex == 0)
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

    public T this[int index]
    {
        get
        {
            if (items != null && index >= 0 && index < items.Count)
            {
                return items[index];
            }
            return default(T);
        }
        set
        {
            if (items == null)
            {
                items = new List<T>();
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
