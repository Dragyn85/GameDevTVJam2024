using UnityEngine;
using UnityEngine.AI;


public class AlianBrain : MonoBehaviour, Damageable
{
    [SerializeField] private int Health = 100;
    [SerializeField] private float AttackTime  = 3;

    private NavMeshAgent     _navMeshAgent;
    private PlayerController _player;
    private GameObject       _playerGO;
    private Animator         _animator;
    private BoxCollider      _boxCollider;
    private ParticleSystem   _particleSystem;
    private WeaponStand      _weaponStand;
    private float            attackTimer;

    void Start()
    {
        attackTimer     = AttackTime;
        _navMeshAgent   = GetComponent<NavMeshAgent>();
        _player         = FindAnyObjectByType<PlayerController>();
        _playerGO       = _player.gameObject;
        _animator       = GetComponent<Animator>();
        _boxCollider    = GetComponent<BoxCollider>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _weaponStand    = FindFirstObjectByType<WeaponStand>();

        _navMeshAgent.destination = _weaponStand.transform.position;
    }


    void Update()
    {
        if (_navMeshAgent.enabled)
        {
            // _navMeshAgent.destination = _weaponStand.transform.position;
        }

        var distance = Vector3.Distance(transform.position, _weaponStand.transform.position);
        if (distance < 2 && isAlive)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer < 0)
            {
                attackTimer = AttackTime;
                AttackStand();
            }
        }
    }

    private void AttackStand()
    {
        _weaponStand.TakeDamage(1);
    }


    public void TakeDamage(int damage)
    {
        _particleSystem.Play();
        if (isAlive)
        {
            Debug.Log("Alian hit!");
            Health -= damage;

            if (Health<=0)
            {
                UnaliveAlien();
                _player.AlianUnalived(this.gameObject);
            }
        }
        else
        {
            Debug.Log($"{name} collider hit");
        }
    }
    
    
    void UnaliveAlien()
    {
        var size   = _boxCollider.size;
        var center = _boxCollider.center;

        size.y   = .8f;

        center.x = .67f;
        center.y = .4f;

        _boxCollider.size   = size;
        _boxCollider.center = center;

        _boxCollider.enabled = false;
        
        Health = 0;
        _animator.SetBool("Alive", false);
        _navMeshAgent.enabled = false;
        Debug.Log($"{name} Unalived");
    }


    public bool isAlive
    {
        get {  return Health > 0; }
    }
}
