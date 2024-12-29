using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class NoReserveWarningModal : UIElement
{
    public bool focusOn = false;
    public TMP_Text description;


    public void Start()
    {
        InitListeners();
        Hide();
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


    public override void OnPointerEnter(PointerEventData data)
    {
        focusOn = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        focusOn = false;
    }

    public override void Hide()
    {
        base.Hide();
        EventMaster.current.OnUIPanelToggle(false);
    }

    public override void Show()
    {
        base.Show();
        EventMaster.current.OnUIPanelToggle(true);
    }
}
