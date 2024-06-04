using System;
using UnityEngine;

public class Round : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 30);
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.linearVelocity.magnitude<1)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        Damageable damageable = other.gameObject.GetComponent<Damageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
            Destroy(this.gameObject);
        }
    }
}
