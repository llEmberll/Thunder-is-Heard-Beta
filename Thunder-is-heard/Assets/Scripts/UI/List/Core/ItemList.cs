using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemList : UIElement, IFillable
{
    public bool focusOn = false;
    public Transform content;


    public virtual void Awake()
    {

    }

    public virtual void Start()
    {
        InitListeners();
        FillContent();
    }

    public virtual void Update()
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
        Debug.Log(gameObject.name + " hide, camera ON");

        base.Hide();
        EventMaster.current.OnUIPanelToggle(false);
    }

    public override void Show()
    {
        Debug.Log(gameObject.name + " show, camera OFF");

        base.Show();
        EventMaster.current.OnUIPanelToggle(true);
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

    public virtual void ClearItems()
    {
        GameObject[] children = content.gameObject.GetComponentsInChildren<Transform>(true)
            .Where(obj => obj != content)
            .Select(obj => obj.gameObject)
            .ToArray();

        foreach (GameObject child in children)
        {
            Destroy(child);
        }
    }

    public virtual void InitListeners()
    {
        EnableListeners();
    }

    public virtual void EnableListeners()
    {
        EventMaster.current.ToggledToBuildMode += Hide;
    }

    public virtual void DisableListeners()
    {
        EventMaster.current.ToggledToBuildMode -= Hide;
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
