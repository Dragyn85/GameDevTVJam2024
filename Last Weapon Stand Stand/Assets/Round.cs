using UnityEngine;

public class Round : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.linearVelocity.magnitude<1)
            Destroy(this.gameObject);
    }
}