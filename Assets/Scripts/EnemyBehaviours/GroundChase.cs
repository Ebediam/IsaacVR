using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BOIVR
{
    public class GroundChase : EnemyBehaviour
    {
        // Start is called before the first frame update
        public NavMeshAgent agentController;

        public override void Initialize()
        {
            base.Initialize();
            agentController.enabled = true;
            agentController.speed = 0.1f;
            agentController.acceleration = 1f;
            agentController.angularSpeed = 200000f;


        }

        public override void Action()
        {

            agentController.destination = enemyController.target.position;

            
            enemyController.rb.AddForce(transform.forward * enemyController.data.acceleration, ForceMode.Acceleration);

            if (enemyController.ignoreMaxSpeed)
            {
                return;
            }

            if (enemyController.rb.velocity.magnitude > enemyController.maxSpeed)
            {
                enemyController.rb.velocity *= (enemyController.maxSpeed / enemyController.rb.velocity.magnitude);
            }


        }


    }
}

