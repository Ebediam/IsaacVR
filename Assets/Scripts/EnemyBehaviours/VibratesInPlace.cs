﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibratesInPlace : EnemyBehaviour
{
    public Vector3 startPosition;

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

        enemyController.rb.AddForce(direction * enemyController.data.acceleration, ForceMode.Acceleration);

        if(enemyController.rb.velocity.magnitude > enemyController.data.maxSpeed)
        {
            enemyController.rb.velocity *= (enemyController.data.maxSpeed / enemyController.rb.velocity.magnitude);
        }
    }
}
