using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : Item
{
    public enum StatBoost
    {
        FireRate,
        BulletSpeed,
        BulletDamage,
        Health,
        Speed,
    }

    public PowerupData data;

    // Start is called before the first frame update
    void Start()
    {
        OnItemPickup += PowerUp;

    }

    public void PowerUp()
    {
        switch (data.boostedStat)
        {
            case StatBoost.BulletDamage:
                Player.local.data.damageBoost += data.amount;
                break;

            case StatBoost.BulletSpeed:
                Player.local.data.bulletSpeedBoost += data.amount;
                break;

            case StatBoost.FireRate:
                Player.local.data.fireRateBoost += data.amount;
                break;

            case StatBoost.Health:
                Player.local.data.healthBoost += data.amount;
               
                Player.local.health += data.amount;
                Player.UpdateHealth();

                break;

            case StatBoost.Speed:
                Player.local.data.movementBoost += data.amount;
                break;





        }

        OnItemPickup -= PowerUp;
        DespawnItem();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
