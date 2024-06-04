using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;


public class AlianBrain : MonoBehaviour, Damageable
{
    [SerializeField] private int        Health     = 100;
    [SerializeField] private float      AttackTime = 3;
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private float      disappearAfterDeathTime = 1.0f;
    
    private NavMeshAgent     _navMeshAgent;
    private PlayerController _player;
    private GameObject       _playerGO;
    private Animator         _animator;
    private BoxCollider      _boxCollider;
    private ParticleSystem   _particleSystem;
    private WeaponStand      _weaponStand;
    private float            attackTimer;
    private IAlienCounter    _alienCounter;


    private WaypointArea[] _waypointAreas;

    private int     currentWaypointIndex = 0;
    private Vector3 currentDestination;
    
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
        _alienCounter   = GameObjectExtensions.FindObjectsOfTypeWithInterface<IAlienCounter>()[0];

        _waypointAreas = FindObjectsByType<WaypointArea>(FindObjectsSortMode.None);
        _waypointAreas = _waypointAreas.OrderBy(wp => -wp.transform.position.z).ToArray();


           currentDestination = _waypointAreas[currentWaypointIndex].GetRandomWaypointInArea();
           _navMeshAgent.destination = currentDestination;
    }


    void Update()
    {
        if (_navMeshAgent.enabled)
        {
            // _navMeshAgent.currentDestination = _weaponStand.transform.position;
        }
        //_navMeshAgent.destination = _weaponStand.transform.position;

        
        var distance = Vector3.Distance(transform.position, currentDestination);
        if (distance < 2 && isAlive)
        {
            if (currentWaypointIndex == 0)
            {
                currentWaypointIndex++;
                currentDestination = _weaponStand.transform.position;
                _navMeshAgent.destination = currentDestination;
            }
            else
            {

                attackTimer -= Time.deltaTime;
                if (attackTimer < 0)
                {
                    attackTimer = AttackTime;
                    AttackStand();
                }
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
    
    
    internal void UnaliveAlien()
    {
        if (_animator.GetBool("Alive") == false)
            return;
        
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
        
        _alienCounter.AdjustCount(-1);
        
        DisappearAlien(disappearAfterDeathTime);
    }


    void DisappearAlien(float time)
    {
        Destroy(this.gameObject, time);
    }

    public bool isAlive
    {
        get {  return Health > 0; }
    }
}
