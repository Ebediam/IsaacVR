using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{

    [CreateAssetMenu]
    public class GunData : ItemData
    {
        public enum GunMode
        {
            Disabled,
            Manual,
            Automatic
        }

        [Header("Main")]
        public GunMode mainGunMode;
        public GameObject bulletPrefab;
        public float fireRate;
        public float bulletSpeed;
        public float bulletDamage;
        public int magCapacity;
        public AudioClip shotSFX;

        [Header("Alt")]
        public GunMode altGunMode;
        public GameObject altBulletPrefab;
        public float altFireRate;
        public float altBulletSpeed;
        public float altBulletDamage;
        public int altMagCapacity;
        public AudioClip altShotSFX;





    }

}
