using UnityEngine;

public class AmmoUpgradeShop : ShopUpgrade
{
    AudioSource shopSound;
    
    protected override void OnUpgrade(IUpgrade pickup)
    {
        shopSound.Play();
    }
}
public class HealReplenishUpgradeShop : ShopUpgrade
{
    AudioSource shopSound;
    
    protected override void OnUpgrade(IUpgrade pickup)
    {
        shopSound.Play();
    }
}