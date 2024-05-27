using UnityEngine;
using UnityEngine.AI;


public class AlianBrain : MonoBehaviour, Damageable
{
    private NavMeshAgent _navMeshAgent;
    private GameObject   _player;

    private int Health = 100;
    
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player       = GameObject.FindAnyObjectByType<PlayerController>().gameObject;
    }

    
    void Update()
    {
        _navMeshAgent.destination = _player.transform.position;
    }
    

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
            Destroy(this.gameObject);
        }
        Debug.Log("Alian hit!");
    }
}
