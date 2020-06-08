using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BOIVR
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class GroundChase : EnemyBehaviour
    {
        // Start is called before the first frame update
        public NavMeshAgent agentController;

        public override void Initialize()
        {
            base.Initialize();
            agentController.enabled = true;
            agentController.speed = enemyController.data.maxSpeed;
            agentController.acceleration = enemyController.data.acceleration;
            agentController.angularSpeed = 200000f;
            agentController.autoRepath = false;

           /*
            agentController.updatePosition = false;
            agentController.updateRotation = false;

            */

            
        }

        public override void Action()
        {
            agentController.SetDestination(enemyController.target.position);


        }


    }
}

