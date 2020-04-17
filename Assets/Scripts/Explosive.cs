using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{

    public ParticleSystem explosionVFX;
    public List<Damageable> inRangeDamageables;

    public ExplosiveData data;

    public SphereCollider sphereCollider;

    public List<Explosive> inRangeExplosives;

    public delegate void ExplosionDelegate(Explosive source);
    public ExplosionDelegate ExplosionEvent;

    public bool alreadyExploded;

    [HideInInspector] Damageable currentDamageable;

    // Start is called before the first frame update
    public void Start()
    {

        sphereCollider.radius = data.explosionRadius;

        inRangeExplosives = new List<Explosive>();

        if(data.target == ExplosiveData.Target.Player)
        {
            return;
        }
        inRangeDamageables = new List<Damageable>();

        Damageable damageable = GetComponent<Damageable>();
        if (damageable)
        {
            damageable.DamageableDestroyedEvent += OnDeath;
        }

    }

    // Update is called once per frame

    

    private void OnTriggerEnter(Collider other)
    {

        if (alreadyExploded)
        {
            return;
        }


        Explosive explosive = other.gameObject.GetComponentInParent<Explosive>();
        if (explosive)
        {
            if(inRangeExplosives.Count > 0)
            {
                if (inRangeExplosives.Contains(explosive))
                {
                    return;
                }
            }
            inRangeExplosives.Add(explosive);
            explosive.ExplosionEvent += ChainExplosion;

            return;
        }

        if (data.target == ExplosiveData.Target.Player)
        {
            return;
        }

        Damageable damageable = other.gameObject.GetComponentInParent<Damageable>();

        if (!damageable)
        {
            return;
        }

        if (inRangeDamageables.Count > 0)
        {
            if (inRangeDamageables.Contains(damageable))
            {
                return;
            }
        }



        inRangeDamageables.Add(damageable);
        damageable.DamageableDestroyedEvent += OnDamageableDestroyed;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!data.explodesOnShoot)
        {
            return;
        }

        if (alreadyExploded)
        {
            return;
        }

        AllBullet bullet = collision.gameObject.GetComponent<AllBullet>();

        if (!bullet)
        {
            return;
        }




        Explode();
    }

    private void OnTriggerExit(Collider other)
    {
        if (alreadyExploded)
        {
            return;
        }


        Explosive explosive = other.gameObject.GetComponentInParent<Explosive>();
        if (explosive)
        {
            if (inRangeExplosives.Count > 0)
            {
                if (inRangeExplosives.Contains(explosive))
                {
                    inRangeExplosives.Remove(explosive);
                    explosive.ExplosionEvent -= ChainExplosion;
                    return;
                }
            }

        }


        if (data.target == ExplosiveData.Target.Player)
        {
            return;
        }

        if (inRangeDamageables.Count == 0)
        {
            return;
        }

        Damageable damageable = other.gameObject.GetComponentInParent<Damageable>();

        if (!damageable)
        {
            return;
        }

        inRangeDamageables.Remove(damageable);
        damageable.DamageableDestroyedEvent -= OnDamageableDestroyed;
    }

    public void ChainExplosion(Explosive source)
    {
        if (alreadyExploded)
        {
            return;
        }

        source.ExplosionEvent -= ChainExplosion;
        Invoke("Explode", Random.Range(0.15f, 0.30f));

    }

    public void Explode()
    {
        if (alreadyExploded)
        {
            return;
        }

        alreadyExploded = true;
        explosionVFX.transform.parent = null;
        explosionVFX.Play();
        Destroy(explosionVFX.gameObject, 2f);

 
        if(data.target != ExplosiveData.Target.Player)
        {
            Debug.Log("Damageable loop starts");
            foreach (Damageable damageable in inRangeDamageables)
            {
                if (!damageable)
                {
                    continue;
                }

                currentDamageable = damageable;

                Vector3 explosionDirection = damageable.transform.position - transform.position;
                float forcePercent = ((data.explosionRadius - explosionDirection.magnitude) / data.explosionRadius);
                
                explosionDirection = explosionDirection.normalized;

                damageable.rb.AddForce(explosionDirection * data.explosionForce * forcePercent, ForceMode.Impulse);
                damageable.TakeDamage(data.maxDamage * forcePercent);
            }
        }

        currentDamageable = null;

        Debug.Log("Damageable loop ends, player loop starts");
        if(data.target != ExplosiveData.Target.Enemies)
        {
            if (Vector3.Distance(Player.local.head.position, transform.position) < data.explosionRadius)
            {
                Player.local.TakeDamage(10f);
            }
        }

        Debug.Log("Player loop ends, removing listeners");
        foreach(Damageable _damageable in inRangeDamageables)
        {
            _damageable.DamageableDestroyedEvent -= OnDamageableDestroyed;
        }
        Debug.Log("Listeners removed, invoking explosion event");
        ExplosionEvent?.Invoke(this);
        Invoke("DeactivateExplosive", 0.1f);


        /*foreach(Explosive explosive in inRangeExplosives)
        {
            if (explosive.inRangeExplosives.Count > 0)
            {
                if (explosive.inRangeExplosives.Contains(this))
                {
                    explosive.inRangeExplosives.Remove(this);
                }
            }           
            
        }
        foreach (Explosive explosive in inRangeExplosives)
        {
            if (explosive)
            {
                explosive.Explode();
                Destroy(gameObject, 0.5f);
            }

        }*/



    }

    public void OnDeath(Damageable damageable)
    {
        damageable.DamageableDestroyedEvent -= OnDeath;
        Explode();
    }


    public void DeactivateExplosive()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 1f);
    }

    public void OnDamageableDestroyed(Damageable damageable)
    {
        if(damageable == currentDamageable)
        {
            return;
        }

        if(inRangeDamageables.Count > 0)
        {
            if (inRangeDamageables.Contains(damageable))
            {
                inRangeDamageables.Remove(damageable);
            }
        }

    }

}
