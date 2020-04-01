using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{

    public float currentHealth;

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

    public void TakeDamage(float damage)
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
    }

    public void DestroyDamageable()
    {
        Destroy(gameObject, 0.1f);
    }
}
