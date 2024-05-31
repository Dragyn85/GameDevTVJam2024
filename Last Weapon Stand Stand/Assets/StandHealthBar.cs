using UnityEngine;

public class StandHealthBar : MonoBehaviour
{
    [SerializeField] private Transform _healthBarTransform;
    [SerializeField] private float MAX_HEALTH = 100;

    private                  float _health;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = MAX_HEALTH;
    }


    public float Health
    {
        get { return _health;}
        set
        {
            _health = value;
            Vector3 scale = _healthBarTransform.localScale;
            scale.x                        = Health / MAX_HEALTH;
            _healthBarTransform.localScale = scale;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
