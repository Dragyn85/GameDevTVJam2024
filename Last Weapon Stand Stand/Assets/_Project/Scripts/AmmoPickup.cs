using UnityEngine;

public class AmmoPickup : MonoBehaviour , IUpgrade
{
    [SerializeField] private int ammoAmount = 10;
    [SerializeField] int maxAmmo = 1000;
    [SerializeField] int replenishAmount = 10;
    [SerializeField] float replenishTime = 5.0f;
    [SerializeField] float nextReplenishTime = 0.0f;

    [SerializeField,Range(-2,-0.01f)] private float replenishTimeUpgradeAmount = -0.5f;
    [SerializeField] private int replenishAmountUpgradeAmount = 5;

    public void TakeAmmo(Ammo ammoToRefill)
    {Debug.Log("taking ammo");
        ammoToRefill.AddAmmo(ammoAmount);
        ammoAmount = 0;
    }

    private void Awake()
    {
        nextReplenishTime = Time.time + replenishTime;
    }

    private void Update()
    {
        if(Time.time > nextReplenishTime)
        {
            ammoAmount += replenishAmount;
            ammoAmount = Mathf.Clamp(ammoAmount, 0, maxAmmo);
            nextReplenishTime = Time.time + replenishTime;
        }
    }

    public void Upgrade()
    {
        replenishAmount += replenishAmountUpgradeAmount;
        replenishTime += replenishTimeUpgradeAmount;
    }
}

public interface IUpgrade
{
    void Upgrade();
}
