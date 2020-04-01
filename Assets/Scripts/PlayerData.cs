using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public float maxSpeed;
    public float acceleration;
    public float turnAngle;
    public float maxHealth;
    public float invincibilityTime;

    public int coins;
    public int keys;
    public int bombs;

}
