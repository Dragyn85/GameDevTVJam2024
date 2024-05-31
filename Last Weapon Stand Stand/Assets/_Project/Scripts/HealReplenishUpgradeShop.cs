using UnityEngine;

public class HealReplenishUpgradeShop : ShopUpgrade
{
    AudioSource shopSound;
    
    protected override void OnUpgrade(IUpgrade pickup)
    {
        if(shopSound != null)
        {   
            shopSound.Play();
        }
    }
}