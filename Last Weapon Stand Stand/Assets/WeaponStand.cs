using UnityEngine;

public class WeaponStand : MonoBehaviour
{
    [SerializeField] private int   health = 100;
    [SerializeField] private Alarm _alarm;

    private float damageTimmer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        damageTimmer -= Time.deltaTime;
        if (damageTimmer < 0)
        {
            _alarm.AlarmOn = false;
        }
    }

    public void TakeDamage(int damage)
    {
        _alarm.AlarmOn =  true;
        health         -= damage;
        damageTimmer   =  3;
    }
}
