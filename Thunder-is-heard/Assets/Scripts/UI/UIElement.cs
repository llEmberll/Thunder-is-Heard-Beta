using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElement : MonoBehaviour, IPointerEnterHandler
{
    public void Toggle()
    {

        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public virtual void Show()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public virtual void OnPointerEnter(PointerEventData data)
    {
    }
}
