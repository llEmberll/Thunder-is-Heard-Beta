using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using System.Collections.Generic;


public class MissionDetalization: UIElement
{
    public bool focusOn = false;

    public string _id = null;
    public TMP_Text TmpName, TmpDescription;
    public Transform _gives;

    public bool IsClickedOutside()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return !focusOn;
        }

        return false;
    }

    public void UpdateDetalization(string id, string name, string description, ResourcesData givesData)
    {
        _id = id;
        TmpName.text = name;
        TmpDescription.text = description;

        ResourcesProcessor.UpdateResources(_gives, givesData);
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
        this.gameObject.SetActive(false);
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        focusOn = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        focusOn = false;
    }

    public void Load()
    {
        if (_id == null)
        {
            return;
        }

        Mission.Load(_id);
    }
}
