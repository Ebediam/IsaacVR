﻿using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace BOIVR
{
    public class Explosive : MonoBehaviour
    {

        public ParticleSystem explosionVFX;
        public AudioSource explosionSFX;

        public ExplosiveData data;

        public delegate void ExplosionDelegate(Explosive source);

        public ExplosionDelegate ExplosionEvent;

        [HideInInspector] public bool alreadyExploded;

        // Start is called before the first frame update
        public void Start()
        {
            
            Damageable damageable = GetComponent<Damageable>();

            if (damageable)
            {
                damageable.DamageableDestroyedEvent += OnDeath;
            }

            if(data.timerToExplodeOnSpawn != 0)
            {
                Invoke("Explode", data.timerToExplodeOnSpawn);
            }

        }

        // Update is called once per frame

                   

        private void OnCollisionEnter(Collision collision)
        {
            if (alreadyExploded)
            {
                return;
            }

            if (!data.explodesOnContact)
            {
                return;
            }

            switch (collision.gameObject.layer)
            {
                case 8: //Bullets
                case 11: //Enemy bullets
                    if (!data.ignoreBullets)
                    {
                        Explode(collision.GetContact(0).point);
                    }
                    break;

                case 9: //Items
                case 20: //HeldItems
                    if (!data.ignoreItems)
                    {
                        Explode(collision.GetContact(0).point);
                    }
                    break;

                case 10: //Player
                case 18: //Hands
                    if (!data.ignorePlayer)
                    {
                        Explode(collision.GetContact(0).point);
                    }

                    break;

                case 12: //Enemies
                    if (!data.ignoreEnemies)
                    {
                        Explode(collision.GetContact(0).point);
                    }
                    break;

                default:
                    if (!data.ignoreEnvironment)
                    {
                        Explode(collision.GetContact(0).point);
                    }
                    break;

            }
        }

        

        public void ChainExplosion(Explosive source, float delay)
        {
            if (alreadyExploded)
            {
                return;
            }
            Invoke("Explode", delay);

        }


        public void Explode()
        {
            Explode(transform.position);
        }

        public void Explode(Vector3 sourcePoint)
        {
            if (alreadyExploded)
            {
                return;
            }

            alreadyExploded = true;

            if (explosionVFX)
            {
                explosionVFX.transform.parent = null;
                explosionVFX.Play();
                Destroy(explosionVFX.gameObject, 2f);
            }

            if (explosionSFX)
            {
                explosionSFX.transform.parent = null;
                explosionSFX.Play();
                Destroy(explosionSFX.gameObject, 2f);
            }
   

            List<Item> affectedItems = new List<Item>();
            List<Explosive> affectedExplosives = new List<Explosive>();
            List<Enemy> affectedEnemies = new List<Enemy>();

            foreach(Collider col in Physics.OverlapSphere(transform.position, data.explosionRadius, data.collisionLayerMask, QueryTriggerInteraction.Ignore))
            {
                Explosive explosive = col.GetComponentInParent<Explosive>();

                if (explosive)
                {
                    if (explosive.data.canChainExplode)
                    {
                        if (!affectedExplosives.Contains(explosive))
                        {
                            affectedExplosives.Add(explosive);
                            continue;
                        }
                    }

                    
                }

                if (data.affectsEnemies)
                {
                    Enemy enemy = col.GetComponentInParent<Enemy>();

                    if (enemy)
                    {
                        if (!affectedEnemies.Contains(enemy))
                        {
                            affectedEnemies.Add(enemy);
                            continue;
                        }
                    }
                }

                if (data.affectsItems)
                {
                    Item item = col.GetComponentInParent<Item>();

                    if (item)
                    {
                        if (!affectedItems.Contains(item))
                        {
                            affectedItems.Add(item);
                            continue;
                        }
                    }

                }

            }

            if(affectedEnemies.Count > 0)
            {
                foreach (Enemy _enemy in affectedEnemies)
                {
                    Vector3 explosionDirection = _enemy.transform.position - sourcePoint;
                    float forcePercent = ((data.explosionRadius - explosionDirection.magnitude) / data.explosionRadius);
                    explosionDirection = explosionDirection.normalized;
                    _enemy.rb.AddForce(explosionDirection * data.explosionForce * forcePercent, data.forceMode);
                    _enemy.TakeDamage(data.maxDamage * forcePercent);

                }
            }
   

            if(affectedItems.Count > 0)
            {
                foreach (Item _item in affectedItems)
                {
                    Vector3 explosionDirection = _item.transform.position - sourcePoint;
                    float forcePercent = ((data.explosionRadius - explosionDirection.magnitude) / data.explosionRadius);
                    explosionDirection = explosionDirection.normalized;
                    _item.rb.AddForce(explosionDirection * data.explosionForce * forcePercent, data.forceMode);
                    

                }
            }

           if(affectedExplosives.Count > 0)
           {
                foreach(Explosive _explosive in affectedExplosives)
                {
                    Vector3 explosionDirection = _explosive.transform.position - sourcePoint;
                    float forcePercent = ((data.explosionRadius - explosionDirection.magnitude) / data.explosionRadius);
                    _explosive.ChainExplosion(this, 0.5f * forcePercent);


                }
           }

            if (data.affectsPlayer)
            {
                Vector3 explosionDirection = Player.local.headCamera.transform.position - sourcePoint;

                if(explosionDirection.magnitude < data.explosionRadius)
                {
                    explosionDirection = explosionDirection.normalized;
                    float forcePercent = ((data.explosionRadius - explosionDirection.magnitude) / data.explosionRadius);
                    Player.local.rb.AddForce(explosionDirection * data.explosionForce * forcePercent, data.forceMode);
                    Player.local.TakeDamage(10f);
                }


            }

            ExplosionEvent?.Invoke(this);

            if (data.despawnAfterExplosion)
            {
                DeactivateExplosive();
            }

        }

        public void OnDeath(Damageable damageable)
        {
            damageable.DamageableDestroyedEvent -= OnDeath;
            Explode();
        }


        public void DeactivateExplosive()
        {
            gameObject.SetActive(false);
            
        }



    }
}

