using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibratesInPlace : EnemyBehaviour
{
    public Vector3 startPosition;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Utils.randomVector3() + (startPosition - transform.position).normalized;

        direction = direction.normalized;

        rb.AddForce(direction * data.acceleration, ForceMode.Acceleration);

        if(rb.velocity.magnitude > data.maxSpeed)
        {
            rb.velocity *= (data.maxSpeed / rb.velocity.magnitude);
        }
    }
}
