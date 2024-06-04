using System;
using UnityEngine;

public class DissapearAfterSeconds : MonoBehaviour
{
    [SerializeField] private GameObject otherGameObject;
    private void Start()
    {
        Invoke(nameof(Disable), 3);
    }
    
    private void Disable()
    {
        gameObject.SetActive(false);
        
        if (otherGameObject != null)
        {
            otherGameObject.SetActive(true);
        }
    }
}
