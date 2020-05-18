using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class Body : MonoBehaviour
    {
        public Transform head;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(head.position.x, transform.position.y, head.position.z);
        }
    }
}

