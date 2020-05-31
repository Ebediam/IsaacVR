using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class ShootAt : EnemyAction
    {

        public Transform spawnPoint;
        public EnemyBullet bullet;
        public int bulletsPerShot;
        public float delayBetweenBullets;
        public float bulletSpeed;

        // Start is called before the first frame update


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
            EnemyBullet bulletInstantiate = Instantiate(bullet);

            bulletInstantiate.transform.position = spawnPoint.position;
            bulletInstantiate.transform.rotation = spawnPoint.rotation;
            bulletInstantiate.rb.AddForce((enemyController.target.transform.position - spawnPoint.position).normalized * bulletSpeed, ForceMode.VelocityChange);
            bulletInstantiate.damage = enemyController.data.damage;

        }
    }
}

