using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : UIElement
{
    public Sprite _icon;
    public Image _itemImage;

    public TMP_Text TmpName, TmpDescription;

    public string _description;
    public int _count;
    public string _id;
    public string _objName = "";

    public abstract string Type { get; }

    public virtual void Init(string itemId, string itemName, Sprite itemIcon = null)
    {
        _id = itemId; _objName = itemName; _icon = itemIcon;
        _itemImage.sprite = _icon;

        UpdateUI();
    }


    public virtual void Awake()
    {
    }

    public abstract void Interact();

    public virtual void UpdateUI()
    {
        TmpName.text = _objName;
    }

    public virtual void UpdateCount(int newCount)
    {
        if (newCount < 1)
        {
            Destroy(this.gameObject);
            return;
        }

        _count = newCount;
    }

    public void Increment(int number = 1)
    {
        UpdateCount(_count + number);
    }
}
