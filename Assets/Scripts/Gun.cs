using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item
{

    public GunData data;
    public Transform spawnPoint;

    public Material gunSwitchMaterial;

    public bool OnCooldown = false;

    public float timer=0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!OnCooldown) 
        {
            return;
        }

        timer += Time.deltaTime;

        if(timer >= data.fireRate)
        {
            OnCooldown = false;
            timer = 0f;
            gunSwitchMaterial.color = Color.green;
        }
    }

    public override void Use()
    {
        base.Use();
        Shoot();
        
    }

    void Shoot()
    {
        if (OnCooldown)
        {
            
            return;
        }
        GameObject bullet = Instantiate(data.bulletPrefab);
        bullet.transform.position = spawnPoint.position;
        bullet.transform.rotation = spawnPoint.rotation;

        bullet.GetComponent<Bullet>().rb.AddForce(bullet.transform.forward * data.bulletSpeed, ForceMode.VelocityChange);
        bullet.GetComponent<Bullet>().damage = data.bulletDamage;
        OnCooldown = true;
        gunSwitchMaterial.color = Color.red;


    }

}
