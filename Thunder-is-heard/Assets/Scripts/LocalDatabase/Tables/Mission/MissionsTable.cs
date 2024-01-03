using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "missions table", fileName = "missions")]
public class MissionsTable : SomeTable
{
    public List<MissionData> items;
    public MissionData currentItem;

    public new List<MissionData> Items { get { return items; } set { } }

    public override string Name
    {
        get
        {
            return "Mission";
        }
    }

    public void AddElement()
    {
        if (items == null)
        {
            items = new List<MissionData>();
        }

        currentItem = new MissionData();
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

    public MissionData GetNext()
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

    public MissionData GetPrev()
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

    public MissionData this[int index]
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
                Debug.Log("����� �� ������� ������� ��� �������� ������� ��������");
            }
        }
    }
}
