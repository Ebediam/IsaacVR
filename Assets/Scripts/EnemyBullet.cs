using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
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
        Player target = collision.gameObject.GetComponentInParent<Player>();

        if (target)
        {
            target.TakeDamage(damage);
        }

        Destroy(this.gameObject, 0.05f);
    }
}
