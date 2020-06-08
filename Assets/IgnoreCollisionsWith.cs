using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;


namespace BOIVR
{
    public class IgnoreCollisionsWith : MonoBehaviour
    {
        // Start is called before the first frame update
        public List<GameObject> targets;
        void Start()
        {
            foreach(GameObject target in targets)
            {
                Utils.IgnoreCollisionsBetween(gameObject, target);
            }
            
            
        }

    }
}

