using TMPro;
using UnityEngine;

public abstract class Item : UIElement
{
    public Sprite icon;

    public TMP_Text TmpName;

    public string id;
    public string objName = "";

    public virtual string EntityType {
        get
        {
            return "Item";
        }
    }

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
}
