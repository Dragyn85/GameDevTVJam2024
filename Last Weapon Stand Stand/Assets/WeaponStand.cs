using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponStand : MonoBehaviour
{
    [SerializeField] private int            health     = 100;
    [SerializeField] private float          _alarmTime = 3.5f;
    [SerializeField] private Alarm          _alarm;
    [SerializeField] private StandHealthBar _standHealthBar;

    private float damageTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        }
    }

    public void TakeDamage(int damage)
    {
        _alarm.AlarmOn         =  true;
        health                 -= damage;
        damageTimer            =  _alarmTime;
        _standHealthBar.Health =  health;
    }
}
