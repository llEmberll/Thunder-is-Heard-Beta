using System.Collections;
using System.Collections.Generic;

public interface ISubsituableShopBehaviour
{
    public void Init();

    public List<ShopItem> GetItems(Shop conductor);

    public bool IsAvailableToBuy(ShopItem item);
    public void OnBuy(ShopItem item);

}
