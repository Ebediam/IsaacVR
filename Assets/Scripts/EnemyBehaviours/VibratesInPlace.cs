using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibratesInPlace : EnemyBehaviour
{
    public Vector3 startPosition;
    public Rigidbody rb;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        startPosition = transform.position;
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (!enemyController.active)
        {
            return;
        }


        Vector3 direction = Utils.randomVector3() + (startPosition - transform.position).normalized;

        direction = direction.normalized;

        rb.AddForce(direction * enemyController.data.acceleration, ForceMode.Acceleration);

        if(rb.velocity.magnitude > enemyController.data.maxSpeed)
        {
            rb.velocity *= (enemyController.data.maxSpeed / rb.velocity.magnitude);
        }
    }
}
