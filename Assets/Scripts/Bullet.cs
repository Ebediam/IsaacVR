using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public float damage;
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

        Destroy(this.gameObject, 0.05f);
    }
}
