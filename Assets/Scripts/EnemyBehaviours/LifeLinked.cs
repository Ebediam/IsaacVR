using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class LifeLinked : EnemyBehaviour
    {
        public Damageable linkedTo;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            linkedTo.DamageableDestroyedEvent += LifeLinkedDestroyed;
            enemyController.DamageableDestroyedEvent += CleanLifeLink;
            //linkedTo.TakeDamageEvent += LifeLinkedDamaged;
        }

        // Update is called once per frame
        public override void Update()
        {
            if (!enemyController.active)
            {
                return;
            }
            base.Update();





        }
        /*
        public void LifeLinkedDamaged(Damageable damageable, float damage)
        {        
            enemyController.TakeDamage(damage);
        }
        */
        public void LifeLinkedDestroyed(Damageable damageable)
        {
            enemyController.DestroyDamageable();
        }

        public void CleanLifeLink(Damageable damageable)
        {
            enemyController.DamageableDestroyedEvent -= CleanLifeLink;
            linkedTo.DamageableDestroyedEvent -= LifeLinkedDestroyed;
            linkedTo = null;
        }

    }
}

