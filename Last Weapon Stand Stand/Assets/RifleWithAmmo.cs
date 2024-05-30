using UnityEngine;

public class RifleWithAmmo : MonoBehaviour
{
    [SerializeField] private float bulletVelocity = 20f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    [SerializeField] private Ammo ammo;

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
        if (ShootBullet())
        {
            _audioSource.Play();
        }
    }


    private bool ShootBullet()
    {
        if (ammo.TryConsumeAmmo())
        {
            var go = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            var rb = go.GetComponent<Rigidbody>();
            rb.linearVelocity = rb.transform.forward * bulletVelocity;
            return true;
        }

        return false;
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Ammo GetAmmo()
    {
        return ammo;
    }
}