﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{

    public class IceWind : Spell
    {

        public static List<Frozen> frozenEnemies = new List<Frozen>();

        public Collider detectionCollider;

        // Start is called before the first frame update


        public override void OnCast()
        {
            base.OnCast();
            detectionCollider.enabled = true;
        }


        public void OnTriggerEnter(Collider other)
        {
            if (!isCasting)
            {
                return;
            }

            Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();


            if (!enemy)
            {
                return;
            }

            Frozen frozenStatus = other.gameObject.GetComponentInParent<Frozen>();

            if (frozenStatus)
            {
                frozenStatus.isBeingFrozen = true;
            }
            else
            {
                Frozen _frozenStatus = enemy.gameObject.AddComponent<Frozen>();
                _frozenStatus.isBeingFrozen = true;
                frozenEnemies.Add(_frozenStatus);

            }

        }

        private void OnTriggerExit(Collider other)
        {
            Frozen frozenStatus = other.gameObject.GetComponentInParent<Frozen>();

            if (frozenStatus)
            {
                frozenStatus.isBeingFrozen = false;
            }

        }

        public override void OnCastStop()
        {
            base.OnCastStop();
            detectionCollider.enabled = false;
            foreach (Frozen frozenEnemy in frozenEnemies)
            {
                frozenEnemy.isBeingFrozen = false;
            }
        }

    }

}