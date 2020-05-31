using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class EnemyBehaviour : MonoBehaviour
    {

        [HideInInspector] public Enemy enemyController;

        [HideInInspector] public bool initialized;
        

        // Start is called before the first frame update
        public void Start()
        {


            //Initialize();
        }


        public virtual void Initialize()
        {
            initialized = true;
            enemyController = gameObject.GetComponent<Enemy>();
        }

        public virtual void Action()
        {

        }



    }
}

