using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : EnemyBehaviour
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

        if(timer > enemyController.data.actionCooldown)
        {
            SpawnEnemy();
            timer = Random.Range(-enemyController.data.actionCooldownModifier, enemyController.data.actionCooldownModifier);
        }

    }

    public void SpawnEnemy()
    {
        Enemy enemyInstance = Instantiate(enemyToSpawn.prefab).GetComponent<Enemy>();
        enemyInstance.transform.position = spawnPoint.transform.position;
        enemyInstance.transform.rotation = spawnPoint.transform.rotation;
        enemyInstance.enemyManager = enemyController.enemyManager;
        enemyInstance.active = true;

        if(autoInflictedDamage != 0)
        {
            enemyController.TakeDamage(autoInflictedDamage);
        }

        if(spawnerType == SpawnerType.Random)
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
        base.Initialize();
        timer = (enemyController.data.actionCooldown / 2) + Random.Range(-enemyController.data.actionCooldownModifier, enemyController.data.actionCooldownModifier);

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


    }
}
