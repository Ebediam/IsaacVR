using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{

    public class GroundShake : EnemyBehaviour
    {
        // Start is called before the first frame update

        // Update is called once per frame
        public override void Action()
        {

            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection = new Vector3(randomDirection.x, 0, randomDirection.z);

            enemyController.rb.AddForce(randomDirection.normalized * enemyController.data.acceleration, ForceMode.Acceleration);

            enemyController.rb.velocity = Vector3.ClampMagnitude(enemyController.rb.velocity, enemyController.maxSpeed);

        }
    }

}