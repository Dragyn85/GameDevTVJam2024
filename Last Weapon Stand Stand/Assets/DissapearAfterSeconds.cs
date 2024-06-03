using System;
using UnityEngine;

public class DissapearAfterSeconds : MonoBehaviour
{
    private void Start()
    {
        Invoke("Disable", 3);
    }
    
    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
