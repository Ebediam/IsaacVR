using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{

    public class Poisonous : EnemyBehaviour
    {
        public Poison poisonPrefab;
        float timer;
        public Transform spawnPoint;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        public override void Update()
        {
            if (!enemyController.active)
            {
                return;
            }

            base.Update();

            timer += Time.deltaTime;

            if (timer > enemyController.data.actionCooldown)
            {
                Poison poison = Instantiate(poisonPrefab);
                poison.transform.position = spawnPoint.transform.position;
                timer = RandomizeActionTimer();

            }

        }

        public override void Initialize()
        {
            base.Initialize();
            timer = RandomizeActionTimer() + enemyController.data.actionCooldown / 2f;
        }
    }

}