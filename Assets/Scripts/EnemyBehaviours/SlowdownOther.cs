using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{

    public class SlowdownOther : EnemyBehaviour
    {
        public Enemy enemyToSlowdown;
        public float slowdownPercent;

        // Start is called before the first frame update


        public override void Initialize()
        {
            base.Initialize();
            enemyController.DamageableDestroyedEvent += Slowdown;

        }


        public void Slowdown(Damageable damageable)
        {
            enemyToSlowdown.maxSpeed *= slowdownPercent;
            enemyController.DamageableDestroyedEvent -= Slowdown;
        }
    }
}

