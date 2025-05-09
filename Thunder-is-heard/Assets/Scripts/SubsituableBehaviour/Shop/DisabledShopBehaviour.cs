using UnityEngine;

public class DisabledShopBehaviour : BaseSubsituableShopBehaviour
{

    public override void Init(Shop conductor)
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag(Tags.resourcesProcessor).GetComponent< ResourcesProcessor >();
    }

    public override void OnBuy(ShopItem item)
    {
        
    }

    public override void Toggle(Shop conductor)
    {

    }
}
