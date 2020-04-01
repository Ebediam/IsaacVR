using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item
{

    public GunData data;
    public Transform spawnPoint;

    public Material gunSwitchMaterial;

    public bool OnCooldown = false;

    public bool active = true;

    public float timer=0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            return;
        }

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
        GameObject bulletGO = Instantiate(data.bulletPrefab);
        bulletGO.transform.position = spawnPoint.position;
        bulletGO.transform.rotation = spawnPoint.rotation;
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        bullet.rb.velocity = Player.local.rb.velocity/10f;
        bullet.rb.AddForce(bullet.transform.forward * data.bulletSpeed, ForceMode.VelocityChange);
        bullet.damage = data.bulletDamage;
        OnCooldown = true;
        gunSwitchMaterial.color = Color.red;

    }

}
