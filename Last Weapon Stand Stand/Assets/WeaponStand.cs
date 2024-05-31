using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponStand : MonoBehaviour, IUpgrade
{
    [SerializeField] private int            maxHealth = 100;
    [SerializeField] private int            health     = 100;
    [SerializeField] private float          _alarmTime = 3.5f;
    [SerializeField] private Alarm          _alarm;
    [SerializeField] private StandHealthBar _standHealthBar;

    [Header("Upgrade settings")]
    [SerializeField] private int            repairAmount = 1;
    [SerializeField] private float          repairTime = 10;
    [SerializeField] private int            repairAmountIncrease =1;
    [SerializeField] private float          repairTimeDecrease = 0.3f;
    
    private float damageTimer;
    private float nextRepairTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <=0)
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
    }
    
    public void Upgrade()
    {
        repairAmount += repairAmountIncrease;
        repairTime -= repairTimeDecrease;
    }
    
    public void TakeDamage(int damage)
    {
        _alarm.AlarmOn         =  true;
        health                 -= damage;
        damageTimer            =  _alarmTime;
        _standHealthBar.Health =  health;
    }
}
