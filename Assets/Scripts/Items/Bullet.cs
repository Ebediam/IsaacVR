using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : AllBullet
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Damageable target = collision.gameObject.GetComponentInParent<Damageable>();

        if (target)
        {
            target.TakeDamage(damage);
        }
        rb.detectCollisions = false;
        DestroyBullet();

    }

    public void DestroyBullet()
    {

        Destroy(this.gameObject, 0.05f);
    }
}
