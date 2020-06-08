using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class Powerup : Item
    {
        public delegate void PowerupDelegate();
        public static PowerupDelegate PowerupEvent;
        public enum StatBoost
        {
            FireRate,
            BulletSpeed,
            BulletDamage,
            Health,
            Speed,
            Fly
        }

        [HideInInspector] public PowerupData _data;

        // Start is called before the first frame update
        void Start()
        {
            OnItemPickup += PowerUp;
            _data = data as PowerupData;
        }

        public void PowerUp()
        {
            switch (_data.boostedStat)
            {
                case StatBoost.BulletDamage:
                    Player.local.data.damageBoost += _data.amount;
                    break;

                case StatBoost.BulletSpeed:
                    Player.local.data.bulletSpeedBoost += _data.amount;
                    break;

                case StatBoost.FireRate:
                    Player.local.data.fireRateBoost += _data.amount;
                    break;

                case StatBoost.Health:
                    Player.local.data.healthBoost += _data.amount;

                    Player.local.health += _data.amount;
                    Player.UpdateHealth();

                    break;

                case StatBoost.Speed:
                    Player.local.data.movementBoost += _data.amount;
                    break;


                case StatBoost.Fly:
                    Player.local.data.canFly = true;
                    break;
            }

            OnItemPickup -= PowerUp;
            PowerupEvent?.Invoke();
            DespawnItem();


        }

    }
}

