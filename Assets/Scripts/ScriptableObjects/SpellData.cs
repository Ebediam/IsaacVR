using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpellData : InteractableData
{
    public enum SpellMode
    {
        Continuous,
        Burst
    }

    public SpellMode spellMode;
    public GameObject burstProjectilePrefab;

}
