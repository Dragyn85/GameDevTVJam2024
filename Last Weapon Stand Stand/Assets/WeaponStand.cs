using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class WeaponStand : MonoBehaviour, IUpgrade
{
    [SerializeField] private int maxHealth = 100;

    [FormerlySerializedAs("health")] [SerializeField]
    private int _health = 100;

    [SerializeField] private float          _alarmTime = 3.5f;
    [SerializeField] private Alarm          _alarm;
    [SerializeField] private StandHealthBar _standHealthBar;

    [Header("Upgrade settings")] [SerializeField]
    private int repairAmount = 1;

    [SerializeField] private float repairTime           = 10;
    [SerializeField] private int   repairAmountIncreaseMultiplier = 1;
    [SerializeField] private float repairTimeDecrease   = 0.3f;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private int maxRepairAmount = 50;
    [SerializeField] private float minRepairTickRate = 3f;

    private float damageTimer;
    private float numberOfUpgrades;

    private                  float  nextRepairTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            TakeDamage(100);
        }
        
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0)
            {
                _alarm.AlarmOn = false;
            }

            if (Time.time > nextRepairTime)
            {
                _health        += repairAmount;
                _health        =  Mathf.Clamp(_health, 0, maxHealth);
                nextRepairTime =  Time.time + repairTime;
            }
        }
    }

    public void Upgrade()
    {
        repairAmount += repairAmountIncreaseMultiplier* (int)(1.0f/numberOfUpgrades);
        repairTime   -= repairTimeDecrease/numberOfUpgrades;
    }

    public void TakeDamage(int damage)
    {
        if (_health > 0)
        {
            _health        -= damage;

            if (_health <= 0)
            {
                _health        = 0;
                _alarm.AlarmOn = false;
                damageTimer    = 0;

                GameOver();
            }
            else
            {
                damageTimer            = _alarmTime;
               _alarm.AlarmOn =  true;
            }
            
            _standHealthBar.Health = _health;
        }
    }

    private void GameOver()
    {
        var player = FindFirstObjectByType<PlayerController>();
        GameOverPanel.SetActive(true);
        // LeaderBoardManager.Instance.DisableUpdates(false);
        if (LeaderBoardManager.Instance)
        {
            LeaderBoardManager.Instance.AddScore(player._score);
        }

        Invoke(nameof(GotoMainMenu), 4);
    }

    void GotoMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("Main Menu Copy");
    }
}
