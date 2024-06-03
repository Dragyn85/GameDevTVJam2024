using System;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    [SerializeField] float oscillationSpeed = 1;
    [SerializeField] float oscillationAmplitude = 1;
    
    private Vector3 _initialPosition;
    
    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        float oscillation = Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmplitude;
        transform.position = _initialPosition + Vector3.up * oscillation;
    }
}
