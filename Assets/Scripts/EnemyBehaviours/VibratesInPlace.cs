using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class VibratesInPlace : EnemyBehaviour
    {
        public Vector3 startPosition;

        // Start is called before the first frame update
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
            base.Initialize();

            startPosition = transform.position;
        }
    }
}

