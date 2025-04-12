using System.Collections.Generic;

public interface ISubsituableShopBehaviour
{
    public void Init(Shop conductor);

    public List<ShopItem> GetItems(Shop conductor);

    public bool IsAvailableToBuy(ShopItem item);
    public void OnBuy(ShopItem item);

    public void Toggle(Shop conductor);

    public void FillContent(Shop conductor);

}
