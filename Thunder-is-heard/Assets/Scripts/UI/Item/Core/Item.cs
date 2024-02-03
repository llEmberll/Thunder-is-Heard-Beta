using Org.BouncyCastle.Security;
using TMPro;
using UnityEngine;

public abstract class Item : UIElement
{
    public Sprite objectIcon;

    public TMP_Text TmpText;
    public TMP_Text TmpCount;
    
    public virtual string EntityType {
        get
        {
            return "Item";
        }
    }

    public int itemCount = 1;
    public int objectId;
    public string objectName = "";
    public string objectType;


    public void Init(int id, string name, string type, Sprite icon)
    {
        objectId = id;
        objectName = name;
        objectType = type;
        objectIcon = icon;

        UpdateUI();
    }


    public virtual void Awake()
    {
        TmpText = transform.Find("Text").GetComponent<TMP_Text>();
        TmpCount = transform.Find("Count").GetComponent<TMP_Text>();

        UpdateUI();
    }

    public abstract void Interact();

    public void UpdateCount(int newCount)
    {
        itemCount = newCount;
        TmpCount.text = newCount.ToString();
        if (itemCount < 1)
        {
            Destroy(this.gameObject);
        }
    }

    public void UpdateUI()
    {
        TmpText.text = objectName;
        TmpCount.text = itemCount.ToString();
    }

    public void Substract(int number = 1)
    {
        UpdateCount(itemCount - number);
    }

    public void Increment(int number = 1)
    {
        UpdateCount(itemCount + number);
    }
}
