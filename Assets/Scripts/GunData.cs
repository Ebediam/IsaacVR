using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunData : ScriptableObject
{
    public GameObject bulletPrefab;
    public float fireRate;
    public float bulletSpeed;
    public float bulletDamage;
    public int magCapacity;

}
