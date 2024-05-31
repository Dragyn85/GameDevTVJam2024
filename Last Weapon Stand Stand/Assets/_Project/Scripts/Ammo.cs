using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private int   maxAmmoInClip = 30;
    [SerializeField] private int   maxAmmo       = 200000;
    [SerializeField] private int   initialAmmo   = 1000;
    [SerializeField] private int   currentAmmo;
    [SerializeField] private int   currentAmmoInClip;
    [SerializeField] private float reloadTime = 3;

    private TMP_Text interactionText;
    private bool     reloading = false;

    public int CurrentAmmo => currentAmmo;
    public int CurrentAmmoInClip => currentAmmoInClip;
    public int MaxAmmoInClip => maxAmmoInClip;

    public static event Action<Ammo> OnAmmoChanged;

    public bool Reloading
    {
        get { return reloading; }
    }
    
    private void Awake()
    {
        currentAmmoInClip = maxAmmoInClip;
        currentAmmo = initialAmmo;
    }

    private void Start()
    {
        interactionText = FindFirstObjectByType<InteractionText>().GetComponent<TMP_Text>();
        OnAmmoChanged?.Invoke(this);
    }

    public bool TryConsumeAmmo()
    {
        bool canFire = false;
        if (currentAmmoInClip > 0 && !reloading)
        {
            currentAmmoInClip--;
            canFire = true;
            OnAmmoChanged?.Invoke(this);
        }

        return canFire;
    }

    public void AddAmmo(int amount)
    {
        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToPickup = Math.Min(ammoNeeded, amount);
        currentAmmo += ammoToPickup;
        OnAmmoChanged?.Invoke(this);
    }

    public bool TryReload()
    {
        bool canReload = false;

        if(currentAmmo > 0 && currentAmmoInClip < maxAmmoInClip && !reloading)
        {
            StartCoroutine(Reload());
            canReload = true;
        }
        
        return canReload; 
    }

    private IEnumerator Reload()
    {
        Debug.Log("---  Reloading  ---");
        interactionText.text = "---  Reloading  ---";
        reloading            = true;
        yield return new WaitForSeconds(reloadTime);
        int ammoNeeded = maxAmmoInClip - currentAmmoInClip;
        int ammoAvailable = currentAmmo;
        int ammoToReload = Math.Min(ammoNeeded, ammoAvailable);
        currentAmmoInClip += ammoToReload;
        currentAmmo -= ammoToReload;
        
        OnAmmoChanged?.Invoke(this);
        reloading            = false;
    }
}