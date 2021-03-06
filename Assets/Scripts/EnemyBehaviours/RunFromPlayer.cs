﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class RunFromPlayer : EnemyBehaviour
    {
        // Start is called before the first frame update


        // Update is called once per frame
        public override void Action()
        {

            Vector3 awayFromPlayer = transform.position - Player.local.transform.position;
            awayFromPlayer = new Vector3(awayFromPlayer.x, 0f, awayFromPlayer.z);

            enemyController.rb.AddForce(awayFromPlayer.normalized * enemyController.data.acceleration, ForceMode.Acceleration);

            enemyController.rb.velocity = Vector3.ClampMagnitude(enemyController.rb.velocity, enemyController.maxSpeed);


        }
    }
}

