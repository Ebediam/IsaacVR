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

    [Header("Base for all")]
    public GameObject prefab;
    public new string name;


    public float hitPoints;
    public float damage;
    public float maxSpeed;
    public float acceleration;
    public float mass;

    public Target targetType;

    [Header("Pushback after hit")]
    public bool pushback;
    public float pushbackVelocity;

    [Header("If it performs any actions")]
    public float actionCooldown;
    public float actionCooldownModifier;   



}
