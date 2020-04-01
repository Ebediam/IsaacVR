using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public float hitPoints;
    public float maxSpeed;
    public float acceleration;
    public float damage;
    public float baseAttackCooldown;
    public float baseAttackCooldownRangeModifier;
    public float bulletSpeed;
}
