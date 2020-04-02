using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChase : EnemyBehaviour
{
    // Start is called before the first frame update

    public Transform target;
    public override void Start()
    {
        base.Start();
        target = Utils.GetTarget();


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

        Vector3 targetDirection = target.position - transform.position;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, enemyController.data.maxSpeed * Time.deltaTime, 0f);

        newDirection = new Vector3(newDirection.x, 0f, newDirection.z);
        transform.rotation = Quaternion.LookRotation(newDirection);

        enemyController.rb.AddForce(transform.forward * enemyController.data.acceleration, ForceMode.Acceleration);
        if (enemyController.rb.velocity.magnitude > enemyController.data.maxSpeed)
        {
            enemyController.rb.velocity *= (enemyController.data.maxSpeed / enemyController.rb.velocity.magnitude);
        }


    }
}
