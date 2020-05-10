using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunData : ItemData
{
    public enum GunMode
    {
        Manual,
        Automatic
    }

    public enum BulletUse
    {
        Main,
        Alt,
        Both
    }


    public GameObject bulletPrefab;
    public GameObject altBulletPrefab;

    public GunMode gunMode;
    public BulletUse bulletUse;

    public float fireRate;
    public float bulletSpeed;
    public float bulletDamage;
    public int magCapacity;

    public AudioClip shotSFX;

}
