using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : EnemyBehaviour
{

    Transform target;
    public Rigidbody rb;
    public Enemy enemy;
    // Start is called before the first frame update


    public override void Start()
    {
        base.Start();
        GetTarget();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (!enemyController.active)
        {
            return;
        }

        if (!target)
        {
            GetTarget();
            return;
        }

        transform.LookAt(target);
        rb.AddRelativeForce(Vector3.forward * enemyController.data.acceleration, ForceMode.Acceleration);
        if(rb.velocity.magnitude > enemyController.data.maxSpeed)
        {
            rb.velocity *= (enemyController.data.maxSpeed / rb.velocity.magnitude);
        }


    }

    public void GetTarget()
    {
        if (Player.local)
        {
            target = Player.local.head;
        }
    }
}
