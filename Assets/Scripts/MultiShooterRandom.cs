using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShooterRandom : EnemyBehaviour
{
    // Start is called before the first frame update

    float timer = 0f;

    public List<Transform> spawnPoints;
    public EnemyBullet bullet;
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

        timer += Time.deltaTime;

        if(timer > enemyController.data.shotCooldown)
        {
            for (int i = 0; i <= enemyController.data.bulletsPerShot; i++)
            {
                Invoke("Shoot", enemyController.data.delayBetweenBulletsPerShot * i);
            }
            timer = RandomizeShotTimer();

        }


    }

    public void Shoot()
    {
        foreach(Transform spawnPoint in spawnPoints)
        {
            EnemyBullet bulletInstantiate = Instantiate(bullet);
            bulletInstantiate.transform.position = spawnPoint.position;
            bulletInstantiate.transform.rotation = spawnPoint.rotation;
            bulletInstantiate.rb.AddForce(bulletInstantiate.transform.forward * enemyController.data.bulletSpeed, ForceMode.VelocityChange);
            bulletInstantiate.damage = enemyController.data.damage;

        }
    }

    public override void Initialize()
    {
        base.Initialize();
        timer = RandomizeShotTimer();
    }
}
