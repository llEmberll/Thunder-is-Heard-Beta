using UnityEngine;

public class FocusController : MonoBehaviour
{
    public void Awake()
    {
        EventMaster.current.ObjectFocused += OnFocus;
        EventMaster.current.ClearObjectFocused += OnDefocus;
    }


    public void OnFocus(FocusData focusData)
    {

    }

    public void OnDefocus()
    {

    }
}
