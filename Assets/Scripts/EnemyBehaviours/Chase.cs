using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : EnemyBehaviour
{

    public Transform target;

    // Start is called before the first frame update


    public override void Start()
    {
        base.Start();
        if (!target)
        {
            target = Utils.GetTarget();
        }


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
            target = Utils.GetTarget();
            return;
        }

        transform.LookAt(target);
        enemyController.rb.AddRelativeForce(Vector3.forward * enemyController.data.acceleration, ForceMode.Acceleration);
        if(enemyController.rb.velocity.magnitude > enemyController.data.maxSpeed)
        {
            enemyController.rb.velocity *= (enemyController.data.maxSpeed / enemyController.rb.velocity.magnitude);
        }


    }


}
