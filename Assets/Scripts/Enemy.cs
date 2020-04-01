using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable
{

    public EnemyData data;
    public Room room;
    public bool active = false;

    void Start()
    {

        currentHealth = data.hitPoints;
        room = GetComponentInParent<Room>();

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
        room.EnemyKilled();
        base.DestroyDamageable();
    }
}
