using System;
using TMPro;
using UnityEngine;

public class AmmoText : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text magazineAmmoText;
    
    private void Start()
    {
        Ammo.OnAmmoChanged += UpdateAmmoText;
    }

    private void UpdateAmmoText(Ammo ammo)
    {
        int maxAmmo = ammo.MaxAmmoInClip;
        ammoText.text = ammo.CurrentAmmo.ToString();
        magazineAmmoText.text = $"{ammo.CurrentAmmoInClip} / {maxAmmo}";
    }
    
}
