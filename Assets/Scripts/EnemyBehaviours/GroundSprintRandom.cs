using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSprintRandom : EnemyBehaviour
{

    float coolDownTimer = 0f;
    float actionTimer = 0f;
    bool sprinting = false;
    Vector3 sprintDirection;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void Initialize()
    {
        base.Initialize();
        coolDownTimer = Random.Range(-enemyController.data.actionCooldownModifier, enemyController.data.actionCooldownModifier);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!enemyController.active)
        {
            return;
        }

        base.Update();

        coolDownTimer += Time.deltaTime;

        if(coolDownTimer > enemyController.data.actionCooldown)
        {
            StartSprint();
            coolDownTimer = Random.Range(-enemyController.data.actionCooldownModifier, enemyController.data.actionCooldownModifier);
        }

        if (sprinting)
        {
            Sprint();
            actionTimer += Time.deltaTime;

            if(actionTimer > enemyController.data.actionDuration)
            {
                actionTimer = 0f;
                sprinting = false;
            }
        }



    }


    public void StartSprint()
    {
        sprintDirection = Utils.RandomVector3(false, true, false);
        Vector3 playerDirection = new Vector3((target.position - transform.position).x, 0f, (target.position - transform.position).z);

        float followPlayerPercent = Random.Range(0f, 1f);

        sprintDirection = followPlayerPercent * playerDirection.normalized + (1 - followPlayerPercent) * sprintDirection;     
        sprintDirection = sprintDirection.normalized;
        transform.rotation = Quaternion.LookRotation(sprintDirection, transform.up);

        sprinting = true;
    }
    public void Sprint()
    {     

        enemyController.rb.AddForce(transform.forward * enemyController.data.acceleration, ForceMode.Acceleration);

        if(enemyController.rb.velocity.magnitude > enemyController.maxSpeed)
        {
            enemyController.rb.velocity *= (enemyController.maxSpeed / enemyController.rb.velocity.magnitude);
        }

    }



}
