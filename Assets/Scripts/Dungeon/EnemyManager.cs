﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class EnemyManager : MonoBehaviour
    {
        public delegate void EnemyManagerDelegate();
        public EnemyManagerDelegate AllEnemiesDeadEvent;
        public Room room;

        public List<Enemy> enemies;
        public int totalEnemies;

        public bool clear;

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
            }

        }


        public void NoEnemiesLeft()
        {
            clear = true;
            AllEnemiesDeadEvent?.Invoke();
            room.RoomStartEvent -= AwakeEnemies;
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

