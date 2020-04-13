using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRandomly : EnemyBehaviour
{

    float actionTimer;
    public float maxUpForwardJumpRatio = 3f;
    public float minUpForwardJumpRatio = 1f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        actionTimer = RandomizeActionTimer();


    }

    // Update is called once per frame
    public override void Update()
    {
        if (!enemyController.active)
        {
            return;
        }

        base.Update();

        actionTimer += Time.deltaTime;

        if(actionTimer > enemyController.data.actionCooldown)
        {
            if(enemyController.rb.velocity.y != 0)
            {
                return;
            }


            actionTimer = RandomizeActionTimer();

            ChangeOrientation();
            Jump();
           

        }
        


    }

    public void ChangeOrientation()
    {
        Vector3 newDirection = (Utils.RandomVector3(false, true, false) + 2*Utils.HorizontalVectorToPlayer(transform.position).normalized).normalized;
               

        transform.rotation = Quaternion.LookRotation(newDirection, transform.up);

    }

    public void Jump()
    {
        float randomJumpForce = Random.Range(enemyController.maxSpeed / 1.5f, enemyController.maxSpeed);
        float randomUpRatio = Random.Range(minUpForwardJumpRatio, maxUpForwardJumpRatio);


        enemyController.rb.AddForce((transform.forward + randomUpRatio * transform.up) * randomJumpForce, ForceMode.VelocityChange);

    }
}
