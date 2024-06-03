using System;
using UnityEngine;

public class AmmoPickup : MonoBehaviour , IUpgrade, IInteractable
{
    [SerializeField] private int ammoAmount = 500;
    [SerializeField] int maxAmmo = 1000;
    [SerializeField] int replenishAmount = 10;
    [SerializeField] float replenishTime = 10.0f;
    [SerializeField] float nextReplenishTime = 0.0f;

    [SerializeField,Range(0.01f,2f)] private float replenishTimeUpgradeAmount = -0.5f;
    [SerializeField] private int replenishAmountUpgradeAmount = 5;
    [SerializeField] private string interactionText;
    [SerializeField] private GameObject arrow;

    private float numberOfUpgrades = 1;
    
    
    public event Action OnPickupChanged = delegate { };
    public int AmmoAmount => ammoAmount;
    public int AmmoReplenishAmount => replenishAmount;
    public float AmmoReplenishTime => replenishTime;
    
    public void TakeAmmo(Ammo ammoToRefill)
    {
        ammoToRefill.AddAmmo(ammoAmount);
        ammoAmount = 0;
        arrow.SetActive(false);
        OnPickupChanged?.Invoke();
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
            OnPickupChanged?.Invoke();
        }
    }

    private void Start()
    {
        OnPickupChanged?.Invoke();
    }

    public void Upgrade()
    {
        replenishAmount += (int)(replenishAmountUpgradeAmount * (10.0f/numberOfUpgrades));
        //replenishTime -= replenishTimeUpgradeAmount/numberOfUpgrades;
        
        numberOfUpgrades++;
        OnPickupChanged?.Invoke();
    }

    public string GetInteractionText()
    {
        return $"{interactionText} : {ammoAmount}";
    }
}

public interface IUpgrade
{
    void Upgrade();
}
