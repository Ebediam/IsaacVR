using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{

    public class Poisonous : EnemyAction
    {
        public Poison poisonPrefab;
        public Transform spawnPoint;


        public override void Action()
        {

            Poison poison = Instantiate(poisonPrefab);
            poison.transform.position = spawnPoint.transform.position;
        }

    }

}