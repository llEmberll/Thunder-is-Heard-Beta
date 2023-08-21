using UnityEngine;

public class UIElement : MonoBehaviour
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
}
