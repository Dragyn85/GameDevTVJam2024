using UnityEngine;

public abstract class ShopUpgrade : MonoBehaviour , IInteractable
{
    [SerializeField] private int cost = 100;
    [SerializeField] private GameObject uppgradeObject;
    private IUpgrade upgradable;
    [SerializeField] private string interactionText;

    public double Cost => cost;
    
    private void Awake()
    {
        upgradable = uppgradeObject.GetComponent<IUpgrade>();
    }
    
    public int TryBuy(int currentPoints)
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

    public virtual string GetInteractionText()
    {
        return $"{interactionText} : ${cost}";
    }
}