
public class ItemList : UIElement, IFillable
{
    public virtual void Start()
    {
        EventMaster.current.ToggledToBuildMode += Hide;
        
        FillContent();
    }
    public virtual void FillContent()
    {
        
    }
}
