﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class Coin : Item
    {
        public int coinValue;
        // Start is called before the first frame update
        void Start()
        {
            OnItemPickup += OnPickUp;
        }

        public void OnPickUp()
        {
            OnItemPickup -= OnPickUp;
            Player.local.AddCoins(coinValue+Player.local.data.coins);

            DespawnItem();

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

