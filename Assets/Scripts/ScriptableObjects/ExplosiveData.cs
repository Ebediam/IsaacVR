using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu]
public class ExplosiveData : ScriptableObject
{
    [Header("General settings")]
    public float explosionForce;
    public float explosionRadius;
    public bool despawnAfterExplosion;
    public float cooldownTimer;
    public bool canChainExplode;
    public float maxDamage;
    public ForceMode forceMode;

    [Header("Trigger settings")]
    public float timerToExplodeOnSpawn;
    public float timerToExplodeOnUngrab;
    public bool disallowExplosionWhileGrabbed;
    public bool explodesOnContact;

    [Header("What it affects")]
    public bool affectsEnemies;
    public bool affectsItems;
    public bool affectsPlayer;
    public LayerMask collisionLayerMask;

    [Header("Ignored collisions")]
    public bool ignorePlayer;
    public bool ignoreItems;
    public bool ignoreBullets;
    public bool ignoreEnemies;
    public bool ignoreEnvironment;










}
