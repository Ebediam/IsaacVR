using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable
{
    public EnemyData data;
    [HideInInspector] public EnemyManager enemyManager;
    public bool active = false;
    [HideInInspector] public float maxSpeed;

    void Start()
    {
        currentHealth = data.hitPoints;
        rb.mass = data.mass;
        maxSpeed = data.maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInvincibility();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.isTrigger)
        {
            return;
        }

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
