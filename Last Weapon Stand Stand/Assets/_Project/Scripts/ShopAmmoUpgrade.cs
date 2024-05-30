using UnityEngine;

public abstract class ShopUpgrade : MonoBehaviour 
{
    [SerializeField] private double cost = 100;
    [SerializeField] private GameObject uppgradeObject;
    private IUpgrade upgradable;
    
    public double Cost => cost;
    
    private void Awake()
    {
        upgradable = uppgradeObject.GetComponent<IUpgrade>();
    }
    
    public double TryBuy(double currentPoints)
    {
        Debug.Log("trying to buy");
        if(currentPoints >= cost)
        {
            upgradable.Upgrade();
            OnUpgrade(upgradable);
            return cost;
        }

        return 0;
    } 
    
    protected virtual void OnUpgrade(IUpgrade pickup)
    {
        
    }
}