using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{

    public class JumpRandomly : EnemyAction
    {
        public float maxUpForwardJumpRatio = 3f;
        public float minUpForwardJumpRatio = 1f;

        // Start is called before the first frame update


        // Update is called once per frame
        public override void Action()
        {
            if(enemyController.rb.velocity.y != 0)
            {
                ChangeOrientation();
                Jump();
            }

        }

        public void ChangeOrientation()
        {
            Vector3 newDirection = (Utils.RandomVector3(false, true, false) + 2 * Utils.HorizontalVectorToPlayer(transform.position).normalized).normalized;


            transform.rotation = Quaternion.LookRotation(newDirection, transform.up);

        }

        public void Jump()
        {
            float randomJumpForce = Random.Range(enemyController.maxSpeed / 1.5f, enemyController.maxSpeed);
            float randomUpRatio = Random.Range(minUpForwardJumpRatio, maxUpForwardJumpRatio);


            enemyController.rb.AddForce((transform.forward + randomUpRatio * transform.up) * randomJumpForce, ForceMode.VelocityChange);

        }
    }

}