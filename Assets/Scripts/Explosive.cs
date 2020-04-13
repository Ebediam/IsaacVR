using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : ItemBehaviour
{

    public ParticleSystem explosionVFX;
    public List<Damageable> inRangeDamageables;

    public ExplosiveData data;

    public SphereCollider sphereCollider;

    public List<Explosive> inRangeExplosives;

    public delegate void ExplosionDelegate(Explosive source);
    public ExplosionDelegate ExplosionEvent;

    public bool alreadyExploded;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        sphereCollider.radius = data.explosionRadius;

        inRangeExplosives = new List<Explosive>();

        if(data.target == ExplosiveData.Target.Player)
        {
            return;
        }
        inRangeDamageables = new List<Damageable>();
    }

    // Update is called once per frame
    public override void Update()
    {

        base.Update();


    }

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

    }

    private void OnCollisionEnter(Collision collision)
    {

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
    }

    public void ChainExplosion(Explosive source)
    {
        if (alreadyExploded)
        {
            return;
        }

        source.ExplosionEvent -= ChainExplosion;
        Invoke("Explode", 0.2f);

    }

    public void Explode()
    {
        if (alreadyExploded)
        {
            return;
        }

        explosionVFX.Play();

        if(data.target != ExplosiveData.Target.Player)
        {
            foreach (Damageable damageable in inRangeDamageables)
            {

                Vector3 explosionDirection = damageable.transform.position - transform.position;
                float forcePercent = ((data.explosionRadius - explosionDirection.magnitude) / data.explosionRadius);
                damageable.TakeDamage(data.maxDamage * forcePercent);

                explosionDirection = explosionDirection.normalized;

                damageable.rb.AddForce(explosionDirection * data.explosionForce * forcePercent, ForceMode.Impulse);
            }
        }

        if(data.target != ExplosiveData.Target.Enemies)
        {
            if (Vector3.Distance(Player.local.head.position, transform.position) < data.explosionRadius)
            {
                Player.local.TakeDamage(10f);
            }
        }

        inRangeExplosives.Clear();
        inRangeDamageables.Clear();
        ExplosionEvent?.Invoke(this);
        Invoke("DeactivateExplosive", 0.5f);


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

    public void DeactivateExplosive()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 5f);
    }
}
