using UnityEngine;

public class WeaponStand : MonoBehaviour, IUpgrade
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int   health = 100;
    [SerializeField] private Alarm _alarm;
    [SerializeField] private int repairAmount = 1;
    [SerializeField] private float repairTime = 10;

    [Header("Upgrades")] 
    [SerializeField] private int repairAmountIncrease =1;
    [SerializeField] private float repairTimeDecrease = 0.3f;
    
    private float damageTimmer;
    private float nextRepairTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        damageTimmer -= Time.deltaTime;
        if (damageTimmer < 0)
        {
            _alarm.AlarmOn = false;
        }
        if(Time.time > nextRepairTime)
        {
            health += repairAmount;
            health = Mathf.Clamp(health, 0, maxHealth);
            nextRepairTime = Time.time + repairTime;
        }
    }

    public void TakeDamage(int damage)
    {
        _alarm.AlarmOn =  true;
        health         -= damage;
        damageTimmer   =  3;
    }
    public void Upgrade()
    {
        repairAmount += repairAmountIncrease;
        repairTime -= repairTimeDecrease;
    }
}
