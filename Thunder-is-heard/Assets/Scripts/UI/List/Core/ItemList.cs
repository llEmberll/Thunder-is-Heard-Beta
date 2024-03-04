
public class ItemList : UIElement, IFillable
{
    public virtual void Start()
    {
        OnBuildModeEnable();
        FillContent();
    }

    public virtual void FillContent()
    {
        
    }

    public virtual void OnBuildModeEnable()
    {
        EventMaster.current.ToggledToBuildMode += Hide;
    }
}
