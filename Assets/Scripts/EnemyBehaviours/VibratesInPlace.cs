using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibratesInPlace : EnemyBehaviour
{
    public Vector3 startPosition;
    public float speed;
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

        rb.AddForce(direction * data.speed, ForceMode.Acceleration);

        if(rb.velocity.magnitude > speed)
        {
            rb.velocity *= (speed / rb.velocity.magnitude);
        }
    }
}
