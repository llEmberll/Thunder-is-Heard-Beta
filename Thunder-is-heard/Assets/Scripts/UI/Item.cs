using TMPro;

public abstract class Item : UIElement
{
    
    public TMP_Text TmpText;
    public TMP_Text TmpCount;
    
    public string entityType {
        get
        {
            return "Item";
        }
    }
    
    public string itemName;
    public int itemCount;
    public int objectId;
    public string objectName;


    public void Awake()
    {
        TmpText = transform.Find("Text").GetComponent<TMP_Text>();
        TmpCount = transform.Find("Count").GetComponent<TMP_Text>();

        TmpText.text = objectName;
        TmpCount.text = itemCount.ToString();
    }

    public abstract void Interact();

    public void UpdateCount(int newCount)
    {
        itemCount = newCount;
        TmpCount.text = newCount.ToString();
    }
}
