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
    void Start()
    {
        GetTarget();
        timer = data.baseAttackCooldown/2 + Random.Range(-data.baseAttackCooldownRangeModifier, data.baseAttackCooldownRangeModifier);
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            GetTarget();
            return;
        }

        if (cooldown)
        {
            timer += Time.deltaTime;

            if(timer >= data.baseAttackCooldown)
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
        bulletInstantiate.rb.AddForce(bulletInstantiate.transform.forward * data.bulletSpeed, ForceMode.VelocityChange);
        bulletInstantiate.damage = data.damage;
        cooldown = true;
        timer = Random.Range(-data.baseAttackCooldownRangeModifier, data.baseAttackCooldownRangeModifier);
    }
}
