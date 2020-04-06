using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    [Header("Base stats")]
    public float maxSpeed;
    public float acceleration;
    public float turnAngle;
    public float maxHealth;
    public float invincibilityTime;

    [Header("Modifiers")]
    public float damageBoost;
    public float bulletSpeedBoost;
    public float fireRateBoost;
    public float movementBoost;
    public float healthBoost;

    [Header("Inventory")]
    public int coins;
    public int keys;
    public int bombs;

    [Header("Data")]
    public ItemData bombData;
    public ItemData keyData;
    

    public void ClearModifiers()
    {
        damageBoost = 0f;
        bulletSpeedBoost = 0f;
        fireRateBoost = 0f;
        movementBoost = 0f;
        healthBoost = 0f;
    }

    public void ClearItems()
    {
        coins = 0;
        keys = 0;
        bombs = 0;
    }

}
