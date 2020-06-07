using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public enum Target
    {
        Player,
        NearestEnemy,
        Custom
    }

    [Header("Settings")]
    public new string name;
    public GameObject prefab;
    public Target targetType;

    [Header("Stats")]
    public float hitPoints;
    public float damage;
    public float maxSpeed;
    public float acceleration;
    public WeightClass weightClass;


    [Header("Pushback after hit")]
    public bool pushback;
    public float pushbackVelocity;

    [Header("If it performs any actions")]
    public float actionCooldown;
    public float actionCooldownModifier;   



}
