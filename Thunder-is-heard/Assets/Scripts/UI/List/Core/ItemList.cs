
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemList : UIElement, IFillable
{
    public bool focusOn = false;

    public virtual void Start()
    {
        OnBuildModeEnable();
        FillContent();
    }

    public void Update()
    {
        if (IsClickedOutside())
        {
            OnClickOutside();
        }
    }

    public virtual void OnClickOutside()
    {
        Hide();
    }

    public bool IsClickedOutside()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return !focusOn;
        }

        return false;
    }

    public override void Hide()
    {
        base.Hide();
        EventMaster.current.OnUIListToggle(false);
    }

    public override void Show()
    {
        base.Show();
        EventMaster.current.OnUIListToggle(true);
    }

    public virtual void FillContent()
    {
        
    }

    public static GameObject CreateObject(string prefabPath, Transform parent = null)
    {
        GameObject itemPrefab = Resources.Load<GameObject>(prefabPath);
        GameObject itemObject = Instantiate(itemPrefab);
        itemObject.transform.SetParent(parent,  false);
        return itemObject;
    }

    public virtual void OnBuildModeEnable()
    {
        EventMaster.current.ToggledToBuildMode += Hide;
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        focusOn = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        focusOn = false;
    }
}
