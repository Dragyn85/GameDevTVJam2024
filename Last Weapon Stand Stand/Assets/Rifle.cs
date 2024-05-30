using System;
using Mono.Cecil;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [SerializeField] private float      bulletVelocity = 20f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform  bulletSpawnPoint;

    private AudioSource _audioSource;
    
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnAudioFilterRead(float[] data, int channels)
    {
        
    }

    public void Fire()
    {
        ShootBullet();
        _audioSource.Play();
    }


    private void ShootBullet()
    {
        var go = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        var rb = go.GetComponent<Rigidbody>();
        rb.linearVelocity = rb.transform.forward * bulletVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}