using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item
{

    [HideInInspector]public GunData gunData;
    public Transform spawnPoint;

    public GameObject redCube;
    public GameObject greenCube;

    public bool OnCooldown = false;

    public bool active = true;

    public float timer=0f;

    // Start is called before the first frame update
    void Start()
    {
        Player.EnterSafeZoneEvent += Deactivate;
        Player.ExitSafeZoneEvent += Activate;
        GameManager.GameOverEvent += OnGameOver;
        gunData = data as GunData;
    }

    public void OnGameOver()
    {
        Player.EnterSafeZoneEvent -= Deactivate;
        Player.ExitSafeZoneEvent -= Activate;
        GameManager.GameOverEvent -= OnGameOver;
    }

    public void Deactivate()
    {
        active = false;
    }

    public void Activate()
    {
        active = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!Player.local)
        {
            return;
        }

        if (!active)
        {
            return;
        }

        if (!OnCooldown) 
        {
            return;
        }

        timer += Time.deltaTime;

        if(timer >= (gunData.fireRate+Player.local.data.fireRateBoost))
        {
            OnCooldown = false;
            timer = 0f;
            greenCube.SetActive(true);
            redCube.SetActive(false);
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

        if (!active)
        {
            return;
        }
        GameObject bulletGO = Instantiate(gunData.bulletPrefab);
        bulletGO.transform.position = spawnPoint.position;
        bulletGO.transform.rotation = spawnPoint.rotation;
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        bullet.rb.velocity = Player.local.rb.velocity/10f;
        bullet.rb.AddForce(bullet.transform.forward * (gunData.bulletSpeed+Player.local.data.bulletSpeedBoost), ForceMode.VelocityChange);
        bullet.damage = gunData.bulletDamage + Player.local.data.damageBoost;
        OnCooldown = true;
        redCube.SetActive(true);
        greenCube.SetActive(false);

    }

}
