using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class Chase : EnemyBehaviour
    {


        // Update is called once per frame
        public override void Action()
        {
   
            transform.LookAt(enemyController.target);
            enemyController.rb.AddRelativeForce(Vector3.forward * enemyController.data.acceleration, ForceMode.Acceleration);

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

