using UnityEngine;

public abstract class ShopUpgrade : MonoBehaviour , IInteractable
{
    [SerializeField] private int cost = 100;
    [SerializeField] private float costUpgradeAmount = 1.5f;
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
            cost = (int)(cost * costUpgradeAmount);
            upgradable.Upgrade();
            OnUpgrade(upgradable);
            return cost;
        }

        return 0;
    } 
    
    protected virtual void OnUpgrade(IUpgrade pickup)
    {
        
    }

    public string GetInteractionText()
    {
        return $"{interactionText} : ${cost}";
    }
}
public interface IInteractable
{
    string GetInteractionText();
}