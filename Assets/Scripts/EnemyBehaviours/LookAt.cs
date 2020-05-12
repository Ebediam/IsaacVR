using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class LookAt : EnemyBehaviour
    {

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


            transform.LookAt(target);
        }

    }


}