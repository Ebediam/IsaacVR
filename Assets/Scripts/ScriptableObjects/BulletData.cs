using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BulletData : ScriptableObject
{
    public GameObject prefab;
    public float damage;
    public bool despawnAfterHit;
    public LayerMask collisionLayerMask;

}
