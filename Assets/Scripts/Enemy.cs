using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable
{

    public delegate void EnemyDelegate(Enemy enemy);
    public EnemyDelegate EnemyDeadEvent;

    public EnemyData data;
    public EnemyManager enemyManager;
    public bool active = false;

    void Start()
    {

        currentHealth = data.hitPoints;

    }

    // Update is called once per frame
    void Update()
    {
        CheckInvincibility();
    }

    public void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponentInParent<Player>();

        if (!player)
        {
            return;
        }

        player.TakeDamage(data.damage);
    }

    public override void DestroyDamageable()
    {
        EnemyDeadEvent?.Invoke(this);
        base.DestroyDamageable();
    }
}
