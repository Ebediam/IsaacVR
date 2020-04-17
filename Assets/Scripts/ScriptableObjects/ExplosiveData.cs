using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ExplosiveData : ScriptableObject
{
    public enum Target
    {
        Player,
        Enemies,
        All
    }

    public Target target;
    public float explosionForce;
    public float explosionRadius;
    public float maxDamage;
    public bool explodesOnShoot;
    public bool explodesOnDestroy;



}
