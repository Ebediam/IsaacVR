using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item
{
    public ParticleSystem explosionVFX;
    public List<Damageable> inRangeDamageables;
    public float explosionForce;
    public float explosionRadius;

    public float maxDamage;

    public bool activated = false;
    public float timer;
    public Transform cilinder;
    public Transform cilinderEndPoint;
    public float totalDistance;
    public float step;


    // Start is called before the first frame update
    void Start()
    {
        totalDistance = Vector3.Distance(cilinder.position, cilinderEndPoint.position);
        step = totalDistance / timer;

        inRangeDamageables = new List<Damageable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.gameObject.GetComponentInParent<Damageable>();

        if (!damageable)
        {
            return;
        }

        if(inRangeDamageables.Count > 0)
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
        AllBullet bullet = collision.gameObject.GetComponent<AllBullet>();

        if (!bullet)
        {
            return;
        }

        Explode();
    }

    private void OnTriggerExit(Collider other)
    {

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

    // Update is called once per frame
    void Update()
    {
        if (!activated)
        {
            return;
        }

        timer -= Time.deltaTime;

        cilinder.position = Vector3.MoveTowards(cilinder.position, cilinderEndPoint.position, step*Time.deltaTime);

        if(timer <= 0f)
        {
            Explode();

        }
    }

    public override void Use()
    {
        base.Use();
        InitiateExplosionTimer();
    }

    public void InitiateExplosionTimer()
    {
        activated = true;
        holder.Release();


    }

    public void Explode()
    {
        explosionVFX.Play();

        foreach(Damageable damageable  in inRangeDamageables)
        {
            damageable.TakeDamage(maxDamage);
            Vector3 explosionDirection = damageable.transform.position - transform.position;
            float forcePercent = ((explosionRadius - explosionDirection.magnitude) / explosionRadius);

            explosionDirection = explosionDirection.normalized;

            damageable.rb.AddForce(explosionDirection * explosionForce * forcePercent, ForceMode.Impulse);
        }
        activated = false;
        DespawnItem(0.5f);

    }
}
