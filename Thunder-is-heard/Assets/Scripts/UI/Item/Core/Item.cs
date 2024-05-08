using TMPro;
using UnityEngine;

public abstract class Item : UIElement
{
    public Sprite icon;

    public TMP_Text TmpName, TmpDescription;

    public string description;
    public int count;
    public string id;
    public string objName = "";

    public abstract string Type { get; }

    public virtual void Init(string itemId, string itemName, Sprite itemIcon = null)
    {
        id = itemId; objName = itemName; icon = itemIcon;

        UpdateUI();
    }


    public virtual void Awake()
    {
    }

    public abstract void Interact();

    public virtual void UpdateUI()
    {
        TmpName.text = objName;
    }

    public virtual void UpdateCount(int newCount)
    {
        if (newCount < 1)
        {
            Destroy(this.gameObject);
            return;
        }

        count = newCount;
    }

    public void Increment(int number = 1)
    {
        UpdateCount(count + number);
    }
}
