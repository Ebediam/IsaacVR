using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Enemy : Damageable
{
    public EnemyData data;
    [HideInInspector] public EnemyManager enemyManager;
    public bool active = false;
    [HideInInspector] public float maxSpeed;

   

    public TextMeshPro enemyText;

    float timer2;

    void Start()
    {
        currentHealth = data.hitPoints;
        rb.mass = data.mass;
        maxSpeed = data.maxSpeed;

        if (enemyText)
        {
            enemyText.text = data.name;
        }


    }

    // Update is called once per frame
    void Update()
    {
        CheckInvincibility();
        if (ignoreMaxSpeed)
        {
            timer2 += Time.deltaTime;

            if(timer2 > 0.5f)
            {
                timer2 = 0f;
                ignoreMaxSpeed = false;
            }
        }
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

        if (data.pushback)
        {
            ignoreMaxSpeed = true;
            Pushback();
        }

        
    }

    public void Pushback()
    {

        rb.AddForce(transform.forward * -data.pushbackVelocity, ForceMode.VelocityChange);
    }


    public override void DestroyDamageable()
    {
        base.DestroyDamageable();
    }
}
