using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAt : EnemyBehaviour
{

    Transform target;
    public Transform spawnPoint;
    public EnemyBullet bullet;

    float timer;
    bool cooldown = true;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GetTarget();
        timer = enemyController.data.baseAttackCooldown/2 + Random.Range(-enemyController.data.baseAttackCooldownRangeModifier, enemyController.data.baseAttackCooldownRangeModifier);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (!enemyController.active)
        {
            return;
        }

        if (!target)
        {
            GetTarget();
            return;
        }

        if (cooldown)
        {
            timer += Time.deltaTime;

            if(timer >= enemyController.data.baseAttackCooldown)
            {
                Shoot();

            }
        }


    }

    public void GetTarget()
    {
        if (!Player.local)
        {
            return;
        }

        target = Player.local.head;
    }

    public void Shoot()
    {
        EnemyBullet bulletInstantiate = Instantiate(bullet);

        bulletInstantiate.transform.position = spawnPoint.position;
        bulletInstantiate.transform.rotation = spawnPoint.rotation;
        bulletInstantiate.rb.AddForce(bulletInstantiate.transform.forward * enemyController.data.bulletSpeed, ForceMode.VelocityChange);
        bulletInstantiate.damage = enemyController.data.damage;
        cooldown = true;
        timer = Random.Range(-enemyController.data.baseAttackCooldownRangeModifier, enemyController.data.baseAttackCooldownRangeModifier);
    }
}
