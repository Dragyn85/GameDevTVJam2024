using System;
using UnityEngine;

public class DissapearAfterSeconds : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(Disable), 3);
    }
    
    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
