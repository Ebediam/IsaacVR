using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
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


        public override void CollisionEnterEvent(Collision collision)
        {
            base.CollisionEnterEvent(collision);
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

            gameObject.SetActive(false);
            //Destroy(this.gameObject, 0.05f);
        }
    }
}

