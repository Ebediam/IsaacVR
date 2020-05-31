using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public delegate void DamageableDelegate(Damageable damageable);
    public DamageableDelegate DamageableDestroyedEvent;

    public delegate void TakeDamageDelegate(Damageable damageable, float damage);
    public TakeDamageDelegate TakeDamageEvent;

    [HideInInspector] public float currentHealth;

    public Rigidbody rb;
    public float invencibilityTime = 0.1f;

    [HideInInspector] public bool invicible = false;

    // Start is called before the first frame update



    public virtual void TakeDamage(float damage)
    {
        if (invicible)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            DestroyDamageable();
        }
        else
        {
            TakeDamageEvent?.Invoke(this, damage);
        }


        invicible = true;
        if (gameObject.activeSelf)
        {
            StartCoroutine(InvincibleTimer(invencibilityTime));
        }
        

    }

    public IEnumerator InvincibleTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        invicible = false;
    }

    public virtual void DestroyDamageable()
    {
        DamageableDestroyedEvent?.Invoke(this);
        //Destroy(gameObject, 0.1f);
        gameObject.SetActive(false);
    }
}
