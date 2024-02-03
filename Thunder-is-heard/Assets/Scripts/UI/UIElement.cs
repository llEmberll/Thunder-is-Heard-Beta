using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElement : MonoBehaviour, IPointerEnterHandler
{
    public void Toggle()
    {

        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public virtual void OnPointerEnter(PointerEventData data)
    {
    }
}
