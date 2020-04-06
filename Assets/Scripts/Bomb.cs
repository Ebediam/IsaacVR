using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item
{


    [HideInInspector]public bool activated = false;
    public float timer;
    public Transform cilinder;
    public Transform cilinderEndPoint;
    float totalDistance;
    float step;

    public Explosive explosionController;


    // Start is called before the first frame update
    void Start()
    {
        totalDistance = Vector3.Distance(cilinder.position, cilinderEndPoint.position);
        step = totalDistance / timer;

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
            explosionController.Explode();
            activated = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        AllBullet bullet = collision.gameObject.GetComponent<AllBullet>();

        if (!bullet)
        {
            return;
        }
        if (holder)
        {
            holder.Release();
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

}
