using System;
using TMPro;
using UnityEngine;

public class AmmoboxDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoAmountText;
    [SerializeField] private TMP_Text ammoRefillAmountText;
    

    [SerializeField] private AmmoPickup ammoPickup;

    private void OnEnable()
    {
        ammoPickup.OnPickupChanged += UpdateDisplay;
    }

    private void OnDisable()
    {
        ammoPickup.OnPickupChanged -= UpdateDisplay;
    }

    private void UpdateDisplay()
    {
        ammoAmountText.text = ammoPickup.AmmoAmount.ToString();
        ammoRefillAmountText.text = ammoPickup.AmmoReplenishAmount.ToString();
       }
}
