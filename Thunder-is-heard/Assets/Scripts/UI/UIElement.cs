using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public virtual void Toggle()
    {
        if (this.gameObject.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
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

    public virtual void OnPointerExit(PointerEventData eventData)
    {
    }
}
