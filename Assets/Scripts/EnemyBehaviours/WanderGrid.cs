﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class WanderGrid : EnemyBehaviour
    {

        [HideInInspector]public bool isTurningInCooldown = false;
        public float turnCooldownTime = 0.1f;
        float timer = 0f;

        float movementTimer = 0f;


        public Sensor leftSensor;
        public Sensor rightSensor;
        // Start is called before the first frame update


        // Update is called once per frame
        public override void Action()
        { 

            enemyController.rb.AddForce(transform.forward * enemyController.data.acceleration, ForceMode.Acceleration);

            if (enemyController.rb.velocity.magnitude > enemyController.maxSpeed)
            {
                enemyController.rb.velocity *= (enemyController.maxSpeed / enemyController.rb.velocity.magnitude);
            }


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

