using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
   
    public class AllBullet : Damager
    {
        public Rigidbody rb;
        public Collider bulletCollider;
        public float damage;
        // Start is called before the first frame update

        public void IgnoreCollisionsWithItem(Item item)
        {
            foreach (Collider col in item.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(bulletCollider, col);
            }
        }

    }




}