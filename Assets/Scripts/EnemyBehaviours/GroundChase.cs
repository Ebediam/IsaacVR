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

        public override void Start()
        {
            base.Start();
            agentController.enabled = false;

        }

        // Update is called once per frame
        public override void Update()
        {
            if (!enemyController.active)
            {
                return;
            }
            base.Update();




            agentController.destination = target.position;




            /*Vector3 targetDirection = target.position - transform.position;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, enemyController.data.maxSpeed * Time.deltaTime, 0f);

            newDirection = new Vector3(newDirection.x, 0f, newDirection.z);
            transform.rotation = Quaternion.LookRotation(newDirection);
            */
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

        public override void Initialize()
        {
            base.Initialize();
            //agentController.speed = enemyController.maxSpeed;
            //agentController.acceleration = enemyController.data.acceleration;

            agentController.enabled = true;
            agentController.speed = 0.1f;
            agentController.acceleration = 1f;
            agentController.angularSpeed = 200000f;

        }

    }
}

