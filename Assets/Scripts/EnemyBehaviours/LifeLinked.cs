using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class LifeLinked : EnemyBehaviour
    {
        public Damageable linkedTo;

        public override void Initialize()
        {
            base.Initialize();
            linkedTo.DamageableDestroyedEvent += LifeLinkedDestroyed;
            enemyController.DamageableDestroyedEvent += CleanLifeLink;

        }


        public void LifeLinkedDamaged(Damageable damageable, float damage)
        {        
            enemyController.TakeDamage(damage);
        }
        
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

