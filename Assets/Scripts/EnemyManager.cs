﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void DeadEnemyListener(Enemy enemy)
    {
        if (clear)
        {
            return;
        }

        enemy.EnemyDeadEvent -= DeadEnemyListener;
        totalEnemies--;
        if(totalEnemies <= 0)
        {
            clear = true;
            AllEnemiesDeadEvent?.Invoke();
            room.RoomStartEvent -= AwakeEnemies;
        }

    }


    public void AwakeEnemies()
    {
        if (clear)
        {
            return;
        }

        foreach(Enemy enemy in enemies)
        {
            enemy.active = true;
            enemy.enemyManager = this;
            enemy.EnemyDeadEvent += DeadEnemyListener;
        }
    }
}
