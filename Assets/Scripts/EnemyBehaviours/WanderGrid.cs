using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BOIVR
{
    public class WanderGrid : EnemyBehaviour
    {

        [HideInInspector]public bool isTurningInCooldown = false;
        public float turnCooldownTime = 0.1f;
        float timer = 0f;
        public NavMeshAgent navMeshAgent;
        float movementTimer = 0f;

        public Vector3 roomCenter;
        public float roomSide;

        public Sensor leftSensor;
        public Sensor rightSensor;
        // Start is called before the first frame update


        public override void Initialize()
        {
            base.Initialize();
            navMeshAgent.enabled = true;
            navMeshAgent.speed = enemyController.maxSpeed;
            navMeshAgent.acceleration = enemyController.data.acceleration;
            navMeshAgent.angularSpeed = 1000f;

            roomCenter = transform.position;
            roomSide = 10f;

            if (enemyController.enemyManager)
            {
                if (enemyController.enemyManager.room)
                {
                    roomCenter = enemyController.enemyManager.room.transform.position;
                    if (enemyController.enemyManager.room.roomManager)
                    {
                        if (enemyController.enemyManager.room.roomManager.dungeonGenerator)
                        {
                            roomSide = enemyController.enemyManager.room.roomManager.dungeonGenerator.dungeonData.roomSide;
                        }
                    }
                }
            }

            SetRandomDestination();


        }

        // Update is called once per frame
        public override void Action()
        {

            if(navMeshAgent.remainingDistance < 0.5f)
            {
                SetRandomDestination();
            }
            
            if(navMeshAgent.velocity.magnitude < 0.1f)
            {
                SetRandomDestination();
            }



            return;
            enemyController.rb.AddForce(transform.forward * enemyController.data.acceleration, ForceMode.Acceleration);


            movementTimer += Time.deltaTime;

            if (movementTimer >= enemyController.data.actionCooldown)
            {
                movementTimer = Random.Range(-enemyController.data.actionCooldownModifier, enemyController.data.actionCooldownModifier);

                Turn();
            }


            if (!isTurningInCooldown)
            {
                return;
            }

            timer += Time.deltaTime;

            if (timer >= turnCooldownTime)
            {
                timer = 0f;
                isTurningInCooldown = false;
            }



        }

        void SetRandomDestination()
        {
            Vector3 destination = new Vector3(Random.Range(-roomSide, roomSide), 0, Random.Range(-roomSide, roomSide));
            destination += roomCenter;

            navMeshAgent.SetDestination(destination);            

        }

        void Turn()
        {
            if (leftSensor.blocked)
            {
                transform.Rotate(transform.up, 90f);
            }
            else if (rightSensor.blocked)
            {
                transform.Rotate(transform.up, -90f);
            }
            else
            {

                int rand = Random.Range(0, 2);


                if (rand == 0)
                {
                    transform.Rotate(transform.up, 90f);
                }
                else if (rand == 1)
                {
                    transform.Rotate(transform.up, -90f);
                }
            }

        }


        private void OnTriggerStay(Collider other)
        {
            return;
            if (isTurningInCooldown)
            {
                return;
            }

            if (other.isTrigger)
            {
                return;
            }

            if (other.gameObject.layer == 10) //Player layer
            {
                return;
            }

            Turn();
            isTurningInCooldown = true;
            Debug.Log("LongWorm has collided with " + other.gameObject.name);

        }
    }
}

