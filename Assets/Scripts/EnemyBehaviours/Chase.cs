using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : EnemyBehaviour
{

    Transform target;
    public Rigidbody rb;
    public Enemy enemy;
    // Start is called before the first frame update


    void Start()
    {
        GetTarget();
        enemy = gameObject.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            GetTarget();
            return;
        }

        transform.LookAt(target);
        rb.AddRelativeForce(Vector3.forward * data.acceleration, ForceMode.Acceleration);
        if(rb.velocity.magnitude > data.maxSpeed)
        {
            rb.velocity *= (data.maxSpeed / rb.velocity.magnitude);
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
