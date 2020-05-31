using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class LookAt : EnemyBehaviour
    {
        public override void Action()
        {  
            transform.LookAt(enemyController.target);
        }
    }


}