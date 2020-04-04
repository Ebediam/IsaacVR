using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : EnemyBehaviour
{
    public Enemy enemyToSpawn;
    public Transform spawnPoint;

    float timer;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        timer = (enemyController.data.actionCooldown / 2);

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
        Enemy enemyInstance = Instantiate(enemyToSpawn);
        enemyInstance.transform.position = spawnPoint.transform.position;
        enemyInstance.transform.rotation = spawnPoint.transform.rotation;
        enemyInstance.active = true;
    }
}
