using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class GroundSprintRandom : EnemyAction
    {

        public float actionDuration;
        bool sprinting = false;
        Vector3 sprintDirection;

        public override void Action()
        {
            StartSprint();
            StartCoroutine(EndSprint(actionDuration));
            StartCoroutine(KeepSprinting());

        }


        public IEnumerator EndSprint(float timer)
        {
            yield return new WaitForSeconds(timer);
            sprinting = false;
        }

        public IEnumerator KeepSprinting()
        {
            while (sprinting)
            {
                Sprint();
                yield return null;
            }
            
        }


        public void StartSprint()
        {
            sprintDirection = Utils.RandomVector3(false, true, false);
            Vector3 playerDirection = new Vector3((enemyController.target.position - transform.position).x, 0f, (enemyController.target.position - transform.position).z);

            float followPlayerPercent = Random.Range(0f, 1f);

            sprintDirection = followPlayerPercent * playerDirection.normalized + (1 - followPlayerPercent) * sprintDirection;
            sprintDirection = sprintDirection.normalized;
            transform.rotation = Quaternion.LookRotation(sprintDirection, transform.up);

            sprinting = true;
        }
        public void Sprint()
        {

            enemyController.rb.AddForce(transform.forward * enemyController.data.acceleration, ForceMode.Acceleration);

            if (enemyController.rb.velocity.magnitude > enemyController.maxSpeed)
            {
                enemyController.rb.velocity *= (enemyController.maxSpeed / enemyController.rb.velocity.magnitude);
            }

        }
    }

}
