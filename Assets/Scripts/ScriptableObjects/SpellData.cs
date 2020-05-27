using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpellData : InteractableData
{
    public enum SpellMode
    {
        Continuous,
        Instant
    }

    public enum CastAllowed
    {
        HandFree,
        handFull,
        Both
    }

    public SpellMode spellMode;
    public CastAllowed castAllowed;
    public float manaCost;
    public float cooldown;

}
