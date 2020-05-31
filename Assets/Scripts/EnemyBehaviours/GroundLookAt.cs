using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class GroundLookAt : EnemyBehaviour
    {
        // Start is called before the first frame update



        // Update is called once per frame
        public override void Action()
        {
 
            Vector3 targetDirection = enemyController.target.position - transform.position;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, enemyController.data.maxSpeed * Time.deltaTime, 0f);

            newDirection = new Vector3(newDirection.x, 0f, newDirection.z);
            transform.rotation = Quaternion.LookRotation(newDirection);


        }


    }
}

