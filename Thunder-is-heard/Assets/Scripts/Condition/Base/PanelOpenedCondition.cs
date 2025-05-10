using UnityEngine;

public class PanelOpenedCondition : BasicCondition
{
    public bool firstCheck = true;

    public string _tag;
    public bool opened = false;


    public PanelOpenedCondition(string tag)
    {
        _tag = tag;
    }

    public void FirstComplyCheck()
    {
        firstCheck = false;

        opened = IsPanelOpened();
        EnableListeners();
    }

    public bool IsPanelOpened()
    {
        GameObject panel = GameObject.FindGameObjectWithTag(_tag);
        if (panel == null) return false;
        return panel.activeSelf;
    }

    public void EnableListeners()
    {
        EventMaster.current.UIPanelToggled += SomePanelToggled;
    }

    public void DisableListeners()
    {
        EventMaster.current.UIPanelToggled -= SomePanelToggled;
    }

    public void SomePanelToggled(bool openedNow)
    {
        opened = IsPanelOpened();
    }


    public override bool IsComply()
    {
        if (firstCheck)
        {
            FirstComplyCheck();
        }

        return opened;
    }
}
