using System;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private int maxAmmoInClip = 30;
    private int maxAmmo = 200000;
    private int initialAmmo = 1000;
    private int currentAmmo;
    private int currentAmmoInClip;
    
    public int CurrentAmmo => currentAmmo;
    public int CurrentAmmoInClip => currentAmmoInClip;

    public static event Action OnAmmoChanged;

    private void Awake()
    {
        currentAmmoInClip = maxAmmoInClip;
        currentAmmo = initialAmmo;
    }

    private void OnEnable()
    {
        OnAmmoChanged?.Invoke();
    }

    public bool TryConsumeAmmo()
    {
        bool canFire = false;
        if (currentAmmoInClip > 0)
        {
            currentAmmoInClip--;
            canFire = true;
            OnAmmoChanged?.Invoke();
        }

        return canFire;
    }

    public void AddAmmo(int amount)
    {
        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToPickup = Math.Min(ammoNeeded, amount);
        currentAmmo += ammoToPickup;
        OnAmmoChanged?.Invoke();
    }

    public bool TryReload()
    {
        bool canReload = false;

        if(currentAmmo > 0 && currentAmmoInClip < maxAmmoInClip)
        {
            Reload();
            canReload = true;
        }
        
        return canReload; 
    }

    private void Reload()
    {
        int ammoNeeded = maxAmmoInClip - currentAmmoInClip;
        int ammoAvailable = currentAmmo;
        int ammoToReload = Math.Min(ammoNeeded, ammoAvailable);
        currentAmmoInClip += ammoToReload;
        currentAmmo -= ammoToReload;
        OnAmmoChanged?.Invoke();
    }
}