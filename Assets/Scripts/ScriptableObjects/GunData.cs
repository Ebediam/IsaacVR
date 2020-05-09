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


    public GameObject bulletPrefab;

    public GunMode gunMode;

    public float fireRate;
    public float bulletSpeed;
    public float bulletDamage;
    public int magCapacity;

    public AudioClip shotSFX;

}
