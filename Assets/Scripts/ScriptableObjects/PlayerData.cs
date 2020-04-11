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
    public float baseHealth;
    public float invincibilityTime;
    public float jumpForce;

    [Header("Modifiers")]
    public float damageBoost;
    public float bulletSpeedBoost;
    public float fireRateBoost;
    public float movementBoost;
    public float healthBoost;
    public bool canFly;


    [Header("Inventory/Stats")]
    public int coins;
    public int keys;
    public int bombs;
    public float currentHealth;
    public ItemData leftHandItem;
    public ItemData rightHandItem;


    [Header("Data")]
    public ItemData bombData;
    public ItemData keyData;

    [Header("Settings")]
    public bool completedLevel;
    public LayerMask groundLayer;

    public void ClearModifiers()
    {
        if (completedLevel)
        {

        }
        else
        {
            damageBoost = 0f;
            bulletSpeedBoost = 0f;
            fireRateBoost = 0f;
            movementBoost = 0f;
            healthBoost = 0f;
            canFly = false;


        }

    }

    public void ClearItems()
    {
        if (completedLevel)
        {
            
        }
        else
        {
            coins = 0;
            keys = 0;
            bombs = 0;
            currentHealth = baseHealth;
            leftHandItem = null;
            rightHandItem = null;
        }


    }

}
