using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class EnemyManager : MonoBehaviour
    {
        public delegate void EnemyManagerDelegate();
        public EnemyManagerDelegate AllEnemiesDeadEvent;
        [HideInInspector]public Room room;

        public List<Enemy> enemies;
        [HideInInspector] public int totalEnemies;

        [HideInInspector] public bool clear;

        public Transform rewardSpawn;
        public AudioSource pickupSFX;
        private void Start()
        {
            totalEnemies = enemies.Count;
            room = gameObject.GetComponentInParent<Room>();
            room.RoomStartEvent += AwakeEnemies;
            AllEnemiesDeadEvent += room.RoomClear;


        }

        public void DeadEnemyListener(Damageable enemy)
        {
            if (clear)
            {
                return;
            }

            enemy.DamageableDestroyedEvent -= DeadEnemyListener;
            totalEnemies--;
            if (totalEnemies <= 0)
            {                
                NoEnemiesLeft();
                SpawnReward();
            }

        }


        public void NoEnemiesLeft()
        {
            clear = true;
            AllEnemiesDeadEvent?.Invoke();
            room.RoomStartEvent -= AwakeEnemies;

        }

        public void SpawnReward()
        {
            if (!Player.local)
            {
                return;
            }

            float randomNumber = Random.Range(0f, 1f);

            if(randomNumber > (Player.local.data.baseLuck + Player.local.data.luckBoost))
            {
                return;
            }

            if (room)
            {
                if (room.roomManager)
                {
                    if (room.roomManager.dungeonGenerator)
                    {
                        PoolData itemPoolData;

                        itemPoolData = room.roomManager.dungeonGenerator.dungeonData.itemPoolData;

                        GameObject pickup = Instantiate(itemPoolData.pickupPool[Random.Range(0, itemPoolData.pickupPool.Count)].prefab);
                        pickup.transform.position = rewardSpawn.transform.position;
                        pickup.transform.rotation = rewardSpawn.transform.rotation;
                        GameManager.ChangeMusicVolume(0.3f);
                        pickupSFX.Play();
                        StartCoroutine(ResetMusicLevel(1f));

                    }
                }
            }
        }

        public IEnumerator ResetMusicLevel(float time)
        {
            yield return new WaitForSeconds(time);
            GameManager.ChangeMusicVolume(1f);
        }

        public void AddEnemy(Enemy enemy)
        {
            if (enemies.Contains(enemy))
            {
                Debug.Log("Enemy not included in the manager because it is already present");
            }
            else
            {
                enemies.Add(enemy);
                totalEnemies++;
                enemy.ActivateEnemy(this);
            }

        }

        public void AwakeEnemies()
        {
            if (clear)
            {
                return;
            }

            if (enemies.Count == 0)
            {
                NoEnemiesLeft();
            }

            foreach (Enemy enemy in enemies)
            {
                enemy.ActivateEnemy(this);                

            }
        }
    }
}

