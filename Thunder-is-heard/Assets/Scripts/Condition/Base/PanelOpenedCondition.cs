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
        if (!opened && _isActive)
        {
            EnableListeners();
        }
    }

    public bool IsPanelOpened()
    {
        GameObject panel = GameObjectUtils.FindGameObjectByTagIncludingInactive(_tag);
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

    protected override void OnActivate()
    {
        // При активации проверяем текущее состояние
        if (firstCheck)
        {
            FirstComplyCheck();
        }
        if (!opened)
        {
            // Если уже проверяли и панель не открыта, подписываемся на события
            EnableListeners();
        }
    }
    
    protected override void OnDeactivate()
    {
        DisableListeners();
    }
    
    protected override void OnReset()
    {
        firstCheck = true;
        opened = false;
        DisableListeners();
    }

    public override bool IsComply()
    {
        if (firstCheck && _isActive)
        {
            FirstComplyCheck();
        }

        return opened;
    }

    public override bool IsRealTimeUpdate()
    {
        return true;
    }
}
