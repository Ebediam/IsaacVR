using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class ShootOnDeath : EnemyBehaviour
    {
        public List<Transform> spawnPoints;
        public EnemyBullet bulletPrefab;
        public float bulletSpeed;

        // Start is called before the first frame update

        // Update is called once per frame

        public override void Initialize()
        {
            base.Initialize();
            enemyController.DamageableDestroyedEvent += OnDeath;
  
        }



        public void OnDeath(Damageable damageable)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                EnemyBullet bullet = Instantiate(bulletPrefab);

                bullet.transform.position = spawnPoint.position;
                bullet.transform.rotation = spawnPoint.rotation;
                bullet.rb.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.VelocityChange);
                bullet.damage = enemyController.data.damage;
            }

            enemyController.DamageableDestroyedEvent -= OnDeath;

        }


    }
}

