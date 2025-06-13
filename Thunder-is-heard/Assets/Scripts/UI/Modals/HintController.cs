using TMPro;
using UnityEngine;

public class HintController : MonoBehaviour
{
    public TMP_Text textComponent;

    public void Start()
    {
        EnableListeners();
        Hide();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this .gameObject.SetActive(true);
    }

    public void EnableListeners()
    {
        EventMaster.current.HintSetup += TurnOn;
        EventMaster.current.HintHidden += TurnOff;
    }

    public void DisableListeners()
    {
        EventMaster.current.HintSetup -= TurnOn;
        EventMaster.current.HintHidden -= TurnOff;
    }


    public void TurnOn(string text)
    {
        textComponent.text = text;
        Show();
    }

    public void TurnOff()
    {
        Hide();
    }
}
