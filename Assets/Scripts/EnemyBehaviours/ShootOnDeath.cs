﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class ShootOnDeath : EnemyBehaviour
    {
        public List<Transform> spawnPoints;
        public EnemyBullet bulletPrefab;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            enemyController.DamageableDestroyedEvent += OnDeath;
        }

        // Update is called once per frame

        public void OnDeath(Damageable damageable)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                EnemyBullet bullet = Instantiate(bulletPrefab);

                bullet.transform.position = spawnPoint.position;
                bullet.transform.rotation = spawnPoint.rotation;
                bullet.rb.AddForce(bullet.transform.forward * enemyController.data.bulletSpeed, ForceMode.VelocityChange);
                bullet.damage = enemyController.data.damage;


            }


        }


    }
}

