using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderGrid : EnemyBehaviour
{

    public bool isTurninginCooldown = false;
    public float turnCooldownTime;
    float timer = 0f;

    float movementTimer = 0f;

    public float turnTime;
    public float turnTimeModifier;
    public Sensor leftSensor;
    public Sensor rightSensor;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (!enemyController.active)
        {
            return;
        }

        enemyController.rb.AddForce(transform.forward * enemyController.data.acceleration, ForceMode.Acceleration);

        if(enemyController.rb.velocity.magnitude > enemyController.data.maxSpeed)
        {
            enemyController.rb.velocity *= (enemyController.data.maxSpeed / enemyController.rb.velocity.magnitude);
        }


        movementTimer += Time.deltaTime;

        if(movementTimer >= turnTime)
        {
            movementTimer = Random.Range(-turnTimeModifier, turnTimeModifier);
            
            Turn();
        }


        if (!isTurninginCooldown)
        {
            return;
        }

        timer += Time.deltaTime;

        if(timer >= turnCooldownTime)
        {
            timer = 0f;
            isTurninginCooldown = false;
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
        if (isTurninginCooldown)
        {
            return;
        }

        if (other.isTrigger)
        {
            return;
        }

        Turn();
        isTurninginCooldown = true;
        Debug.Log("LongWorm has collided with "+other.gameObject.name);



    }
}
