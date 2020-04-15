using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    [Header("Base for all")]
    public GameObject prefab;
    public new string name;


    public float hitPoints;
    public float damage;
    public float maxSpeed;
    public float acceleration;
    public float mass;


    [Header("Pushback after hit")]
    public bool pushback;
    public float pushbackVelocity;

    [Header("If it performs any movement action")]
    public float actionCooldown;
    public float actionCooldownModifier;
    public float actionDuration;

    [Header("If it shoots")]
    public float bulletSpeed;
    public int bulletsPerShot;
    public float shotCooldown;
    public float shotCooldownModifier;
    public float delayBetweenBulletsPerShot;


    


}
