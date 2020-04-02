using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public delegate void DamageableDelegate(Damageable damageable);
    public DamageableDelegate DamageableDestroyedEvent;

    public delegate void TakeDamageDelegate(Damageable damageable, float damage);
    public TakeDamageDelegate TakeDamageEvent;

    public float currentHealth;

    public Rigidbody rb;
    public float invencibilityTime = 0.1f;
    public float timer = 0f;
    public bool invicible = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CheckInvincibility()
    {

        if (!invicible)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= invencibilityTime)
        {
            invicible = false;
            timer = 0f;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (invicible)
        {
            return;
        }

        currentHealth -= damage;
        invicible = true;



        if(currentHealth <= 0f)
        {
            DestroyDamageable();
        }
        else
        {
            TakeDamageEvent?.Invoke(this, damage);
        }


    }

    public virtual void DestroyDamageable()
    {
        DamageableDestroyedEvent?.Invoke(this);
        Destroy(gameObject, 0.1f);
    }
}
