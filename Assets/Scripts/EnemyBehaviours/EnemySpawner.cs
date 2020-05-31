using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class EnemySpawner : EnemyAction
    {
        public enum SpawnerType
        {
            Random,
            Defined
        }

        public SpawnerType spawnerType;

        public EnemyPoolData enemyPoolData;
        public EnemyData enemyToSpawn;
        public Transform spawnPoint;
        public float autoInflictedDamage = 0f;

        float timer;
        // Start is called before the first frame update


        // Update is called once per frame
        public override void Action()
        {
            SpawnEnemy();
        }

        public void SpawnEnemy()
        {
            Enemy enemyInstance = Instantiate(enemyToSpawn.prefab).GetComponent<Enemy>();
            enemyInstance.transform.position = spawnPoint.transform.position;
            enemyInstance.transform.rotation = spawnPoint.transform.rotation;

            enemyInstance.ActivateEnemy(enemyController.enemyManager);


            if (autoInflictedDamage != 0)
            {
                enemyController.TakeDamage(autoInflictedDamage);
            }

            if (spawnerType == SpawnerType.Random)
            {
                PickEnemyDataRandom();
            }
        }

        public void PickEnemyDataRandom()
        {
            enemyToSpawn = enemyPoolData.enemyPoolList[Random.Range(0, enemyPoolData.enemyPoolList.Count)];
        }

        public override void Initialize()
        {
            switch (spawnerType)
            {
                case SpawnerType.Defined:
                    if (!enemyToSpawn)
                    {
                        Debug.LogError("EnemySpawner type Defined but no EnemyData found!");
                        return;
                    }
                    break;

                case SpawnerType.Random:
                    PickEnemyDataRandom();
                    break;
            }

            base.Initialize();
        }
    }
}

