using UnityEngine;
using UnityEngine.Serialization;

public class Alarm : MonoBehaviour
{
    [SerializeField] private bool        _alarmOn      = false;
    [SerializeField] private float       _rotationRate = 360;
    [SerializeField] private Transform   axis;
    [SerializeField] private AudioSource _audioSource;

    private Light   light;
    private Vector3 RotationRate;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light          = GetComponentInChildren<Light>();
        RotationRate   = new Vector3(0, _rotationRate, 0);
        updateAlarmState();
    }

    // Update is called once per frame
    void Update()
    {
        if (AlarmOn)
        {
            axis.Rotate(RotationRate * Time.deltaTime);
        }
    }

    void updateAlarmState()
    {
        if (AlarmOn)
        {
            Debug.Log("Play Alarm Sound");
            _audioSource.Play();
        }
        else
        {
            _audioSource.Stop();
        }

        light.enabled = AlarmOn;
    }

    public bool AlarmOn
    {
        get
        {
            return _alarmOn;
        }
        set
        {
            if (AlarmOn != value)
            {
                _alarmOn        = value;
                updateAlarmState();
            }
        }
    }
    
}
