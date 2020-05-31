using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class MultiShooterRandom : EnemyAction
    {
        // Start is called before the first frame update


        public List<Transform> spawnPoints;
        public EnemyBullet bullet;
        public int bulletsPerShot;
        public float delayBetweenBullets;
        public float bulletSpeed;


        // Update is called once per frame
        public override void Action()
        {  
            for (int i = 0; i <= bulletsPerShot; i++)
            {
                Invoke("Shoot", delayBetweenBullets * i);
            }

        }

        public void Shoot()
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                EnemyBullet bulletInstantiate = Instantiate(bullet);
                bulletInstantiate.transform.position = spawnPoint.position;
                bulletInstantiate.transform.rotation = spawnPoint.rotation;
                bulletInstantiate.rb.AddForce(bulletInstantiate.transform.forward * bulletSpeed, ForceMode.VelocityChange);
                bulletInstantiate.damage = enemyController.data.damage;

            }
        }

    }
}

