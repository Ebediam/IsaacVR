using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLookAt : EnemyBehaviour
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

        Vector3 targetDirection = target.position - transform.position;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, enemyController.data.maxSpeed * Time.deltaTime, 0f);

        newDirection = new Vector3(newDirection.x, 0f, newDirection.z);
        transform.rotation = Quaternion.LookRotation(newDirection);


    }


}
