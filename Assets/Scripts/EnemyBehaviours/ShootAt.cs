using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAt : EnemyBehaviour
{

    public Transform spawnPoint;
    public EnemyBullet bullet;

    float timer = 0f;
    bool cooldown = true;
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

        if (cooldown)
        {
            timer += Time.deltaTime;

            if(timer >= enemyController.data.baseAttackCooldown)
            {
                Shoot();

            }
        }


    }
    public override void Initialize()
    {
        base.Initialize();
        timer = enemyController.data.baseAttackCooldown / 2 + Random.Range(-enemyController.data.baseAttackCooldownRangeModifier, enemyController.data.baseAttackCooldownRangeModifier);
    }


    public void Shoot()
    {
        EnemyBullet bulletInstantiate = Instantiate(bullet);

        bulletInstantiate.transform.position = spawnPoint.position;
        bulletInstantiate.transform.rotation = spawnPoint.rotation;
        bulletInstantiate.rb.AddForce((target.transform.position-spawnPoint.position).normalized * enemyController.data.bulletSpeed, ForceMode.VelocityChange);
        bulletInstantiate.damage = enemyController.data.damage;
        cooldown = true;
        timer = Random.Range(-enemyController.data.baseAttackCooldownRangeModifier, enemyController.data.baseAttackCooldownRangeModifier);
    }
}
