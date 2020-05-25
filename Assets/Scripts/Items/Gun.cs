using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BOIVR
{

    public class Gun : Item
    {
        [HideInInspector] public GunData gunData;
        public Transform spawnPoint;

        public GameObject redCube;
        public GameObject greenCube;

        public bool OnCooldown = false;
        public bool OnAltCooldown = false;

        public bool active = true;

        bool automaticEnabled = false;
        bool altAutomaticEnabled = false;

        float timer = 0f;
        float altTimer = 0f;

        public AudioSource shotSFX;
        public ParticleSystem shotVFX;

        public enum BulletType
        {
            Main,
            Alt
        }


        // Start is called before the first frame update
        void Start()
        {
            Player.EnterSafeZoneEvent += Deactivate;
            Player.ExitSafeZoneEvent += Activate;
            GameManager.GameOverEvent += OnGameOver;
            gunData = data as GunData;
            shotSFX.clip = gunData.shotSFX;
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

            if (OnCooldown)
            {
                timer += Time.deltaTime;

                if (timer >= (gunData.fireRate + Player.local.data.fireRateBoost))
                {
                    OnCooldown = false;
                    timer = 0f;
                    greenCube.SetActive(true);
                    redCube.SetActive(false);

                    if (automaticEnabled)
                    {
                        Shoot(BulletType.Main);
                    }

                }
            }

            if (OnAltCooldown)
            {
                altTimer += Time.deltaTime;
                if (altTimer >= (gunData.altFireRate + Player.local.data.fireRateBoost))
                {
                    OnAltCooldown = false;
                    altTimer = 0f;
                    greenCube.SetActive(true);
                    redCube.SetActive(false);

                    if (altAutomaticEnabled)
                    {
                        Shoot(BulletType.Alt);
                    }

                }
            }

            
        }

        public override void AltUse()
        {
            if (gunData.altGunMode == GunData.GunMode.Disabled)
            {
                return;
            }

            base.AltUse();
            ShootCheck(BulletType.Alt);
        }

        public override void Use()
        {
            if (gunData.mainGunMode == GunData.GunMode.Disabled)
            {
                return;
            }
            base.Use();

            ShootCheck(BulletType.Main);

        }

        public void ShootCheck(BulletType bulletType)
        {

            if (!active)
            {
                return;
            }

            switch (bulletType)
            {
                case BulletType.Main:
                    if (OnCooldown)
                    {
                        return;
                    }

                    switch (gunData.mainGunMode)
                    {
                        case GunData.GunMode.Manual:
                            Shoot(bulletType);
                            break;


                        case GunData.GunMode.Automatic:
                            automaticEnabled = true;
                            Shoot(bulletType);                            
                            break;
                    }

                    break;

                case BulletType.Alt:
                    if (OnAltCooldown)
                    {
                        return;
                    }

                    switch (gunData.altGunMode)
                    {
                        case GunData.GunMode.Manual:
                            Shoot(bulletType);
                            break;


                        case GunData.GunMode.Automatic:
                            automaticEnabled = true;
                            Shoot(bulletType);
                            break;
                    }

                    break;


            }


   

            
        }

        public override void StopUsing()
        {
            base.StopUsing();

            if (gunData.mainGunMode == GunData.GunMode.Automatic)
            {
                automaticEnabled = false;
            }

        }

        public override void StopAltUse()
        {
            base.StopAltUse();

            if (gunData.altGunMode == GunData.GunMode.Automatic)
            {
                altAutomaticEnabled = false;
            }

        }
        public AllBullet Shoot(BulletType bulletType)
        {
            GameObject bulletGO;

            switch (bulletType)
            {
                case BulletType.Alt:
                    bulletGO = Instantiate(gunData.altBulletPrefab);
                    OnAltCooldown = true;
                    break;

                default:
                    bulletGO = Instantiate(gunData.bulletPrefab);
                    OnCooldown = true;
                    break;

            }

            bulletGO.transform.position = spawnPoint.position;
            bulletGO.transform.rotation = spawnPoint.rotation;
            AllBullet bullet = bulletGO.GetComponent<AllBullet>();
            bullet.rb.velocity = Player.local.rb.velocity / 10f;

            bullet.IgnoreCollisionsWithItem(this);

            switch (bulletType)
            {
                case BulletType.Alt:
                    bullet.rb.AddForce(bullet.transform.forward * (gunData.altBulletSpeed + Player.local.data.bulletSpeedBoost), ForceMode.VelocityChange);
                    bullet.damage = gunData.altBulletDamage + Player.local.data.damageBoost;
                    break;

                default:
                    bullet.rb.AddForce(bullet.transform.forward * (gunData.bulletSpeed + Player.local.data.bulletSpeedBoost), ForceMode.VelocityChange);
                    bullet.damage = gunData.bulletDamage + Player.local.data.damageBoost;
                    break;

            }
            
            
            redCube.SetActive(true);
            greenCube.SetActive(false);

            shotSFX.Play();
            shotVFX.Play();


            return bullet;

        }

    }

}