using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable
{

    public EnemyData data;
    void Start()
    {
        currentHealth = data.hitPoints;

    }

    // Update is called once per frame
    void Update()
    {
        CheckInvincibility();
    }
}
