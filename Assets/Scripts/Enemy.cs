using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable
{



    public EnemyData data;
    public EnemyManager enemyManager;
    public bool active = false;

    void Start()
    {
        currentHealth = data.hitPoints;
        rb.mass = data.mass;
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
        base.DestroyDamageable();
    }
}
