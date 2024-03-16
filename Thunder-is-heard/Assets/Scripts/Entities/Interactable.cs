using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Interactable: MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public abstract string Type { get; }

    public abstract void OnChangeState(State newState);


    public abstract void OnFocus();
    public abstract void OnDefocus();
    public abstract void OnClick();

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnFocus();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnDefocus();
    }
}