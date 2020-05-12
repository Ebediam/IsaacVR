using System.Collections;
using System.Collections.Generic;
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

        public bool active = true;

        bool automaticEnabled = false;

        public float timer = 0f;

        public AudioSource shotSFX;
        public ParticleSystem shotVFX;

        public enum BulletType
        {
            Main,
            Alt
        }

        BulletType bulletType;

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

            if (!OnCooldown)
            {
                return;
            }

            timer += Time.deltaTime;

            if (timer >= (gunData.fireRate + Player.local.data.fireRateBoost))
            {
                OnCooldown = false;
                timer = 0f;
                greenCube.SetActive(true);
                redCube.SetActive(false);

                if (automaticEnabled)
                {
                    Shoot(bulletType);
                }

            }
        }

        public override void AltUse()
        {
            if (gunData.bulletUse == GunData.BulletUse.Main)
            {
                return;
            }

            base.AltUse();
            bulletType = BulletType.Alt;
            ShootCheck(bulletType);
        }

        public override void Use()
        {
            if (gunData.bulletUse == GunData.BulletUse.Alt)
            {
                return;
            }
            base.Use();

            bulletType = BulletType.Main;
            ShootCheck(bulletType);

        }

        public void ShootCheck(BulletType bulletType)
        {

            if (OnCooldown)
            {
                return;
            }

            if (!active)
            {
                return;
            }

            switch (gunData.gunMode)
            {
                case GunData.GunMode.Manual:
                    Shoot(bulletType);
                    break;


                case GunData.GunMode.Automatic:
                    Shoot(bulletType);
                    automaticEnabled = true;
                    break;

            }
        }

        public override void StopUsing()
        {
            base.StopUsing();

            if (gunData.gunMode == GunData.GunMode.Automatic)
            {
                automaticEnabled = false;
            }

        }

        public AllBullet Shoot(BulletType bulletType)
        {
            GameObject bulletGO;

            switch (bulletType)
            {
                case BulletType.Alt:

                    bulletGO = Instantiate(gunData.altBulletPrefab);
                    break;

                default:
                    bulletGO = Instantiate(gunData.bulletPrefab);
                    break;

            }

            bulletGO.transform.position = spawnPoint.position;
            bulletGO.transform.rotation = spawnPoint.rotation;
            AllBullet bullet = bulletGO.GetComponent<AllBullet>();

            bullet.rb.velocity = Player.local.rb.velocity / 10f;
            bullet.rb.AddForce(bullet.transform.forward * (gunData.bulletSpeed + Player.local.data.bulletSpeedBoost), ForceMode.VelocityChange);
            bullet.damage = gunData.bulletDamage + Player.local.data.damageBoost;
            OnCooldown = true;
            redCube.SetActive(true);
            greenCube.SetActive(false);

            shotSFX.Play();
            shotVFX.Play();

            return bullet;

        }

    }

}