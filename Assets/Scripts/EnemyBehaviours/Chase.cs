using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : EnemyBehaviour
{

    // Start is called before the first frame update


    public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    public override void Update()
    {
        if (!enemyController.active)
        {
            return;
        }
        base.Update();


        transform.LookAt(target);
        enemyController.rb.AddRelativeForce(Vector3.forward * enemyController.data.acceleration, ForceMode.Acceleration);

        if (enemyController.ignoreMaxSpeed)
        {
            return;
        }
        if(enemyController.rb.velocity.magnitude > enemyController.maxSpeed)
        {
            enemyController.rb.velocity *= (enemyController.maxSpeed / enemyController.rb.velocity.magnitude);
        }


    }




}
