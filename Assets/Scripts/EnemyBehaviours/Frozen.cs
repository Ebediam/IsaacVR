using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frozen : EnemyBehaviour
{

    public bool isBeingFrozen;
    public float freezingSpeed = 0.6f;
    public float minSpeedMultiplier = 0.1f;


    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!enemyController.active)
        {
            return;
        }

        base.Update();

        if (isBeingFrozen)
        {
            enemyController.maxSpeed = Mathf.Lerp(enemyController.maxSpeed, enemyController.maxSpeed*minSpeedMultiplier, freezingSpeed*Time.deltaTime);

        }
        else
        {
            if(enemyController.maxSpeed >= enemyController.data.maxSpeed)
            {
                enemyController.maxSpeed = enemyController.data.maxSpeed;

                return;
            }

            else
            {
                enemyController.maxSpeed = Mathf.Lerp(enemyController.maxSpeed, enemyController.data.maxSpeed, freezingSpeed*0.75f * Time.deltaTime);
  


            }
        }

        if (!enemyController.enemyText)
        {
            return;
        }

        float freezePercent = enemyController.maxSpeed / enemyController.data.maxSpeed;

        if(freezePercent < 0.95f)
        {
            enemyController.enemyText.text = enemyController.data.name + " (" + (100f - Mathf.Round(freezePercent * 100)) + "% frozen)";
        }
        else
        {
            enemyController.enemyText.text = enemyController.data.name;
        }


    }
}
