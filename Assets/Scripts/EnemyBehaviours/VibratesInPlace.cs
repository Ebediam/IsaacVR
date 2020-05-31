using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class VibratesInPlace : EnemyBehaviour
    {
        [HideInInspector]public Vector3 startPosition;

        // Start is called before the first frame update

        // Update is called once per frame
        public override void Action()
        {
            Vector3 direction = Utils.RandomVector3() + (startPosition - transform.position).normalized;

            direction = direction.normalized;

            enemyController.rb.AddForce(direction * enemyController.data.acceleration, ForceMode.Acceleration);

            if (enemyController.rb.velocity.magnitude > enemyController.maxSpeed)
            {
                enemyController.rb.velocity *= (enemyController.maxSpeed / enemyController.rb.velocity.magnitude);
            }
        }

        public override void Initialize()
        {
            startPosition = transform.position;
            base.Initialize();
        }
    }
}

