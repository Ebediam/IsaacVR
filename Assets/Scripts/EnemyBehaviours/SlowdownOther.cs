using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownOther : EnemyBehaviour
{
    public Enemy enemyToSlowdown;
    public float slowdownPercent;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        enemyController.DamageableDestroyedEvent += Slowdown;
    }

    /*
    public override void Update()
    {
        if (!enemyController.active)
        {
            return;
        }
        base.Update();
    }
    */
    public void Slowdown(Damageable damageable)
    {
        enemyToSlowdown.maxSpeed *= slowdownPercent;
        enemyController.DamageableDestroyedEvent -= Slowdown;
    }
}
