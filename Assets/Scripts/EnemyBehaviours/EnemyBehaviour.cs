using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class EnemyBehaviour : MonoBehaviour
    {

        [HideInInspector] public Enemy enemyController;
        [HideInInspector] public Transform target;
        protected bool initialized;


        // Start is called before the first frame update
        public virtual void Start()
        {
            enemyController = gameObject.GetComponent<Enemy>();
        }

        // Update is called once per frame
        public virtual void Update()
        {
            if (!initialized)
            {
                Initialize();
            }
        }

        public virtual void Initialize()
        {
            if (!target)
            {
                target = Utils.GetTarget();
            }
            initialized = true;
        }

        public float RandomizeActionTimer()
        {
            float actionTimer = Random.Range(-enemyController.data.actionCooldownModifier, enemyController.data.actionCooldownModifier);

            return actionTimer;

        }

        public float RandomizeShotTimer()
        {
            float shotTimer = Random.Range(-enemyController.data.shotCooldownModifier, enemyController.data.shotCooldownModifier);

            return shotTimer;
        }



    }
}

