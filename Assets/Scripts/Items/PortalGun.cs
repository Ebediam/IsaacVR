using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class PortalGun : MonoBehaviour
    {
        public static Portal bluePortal;
        public static Portal orangePortal;

        public Portal bluePortalLocal;
        public Portal orangePortalLocal;
        // Start is called before the first frame update
        void Start()
        {
            if (!bluePortal)
            {
                bluePortal = bluePortalLocal;
                bluePortal.transform.parent = null;
            }

            if (!orangePortal)
            {
                orangePortal = orangePortalLocal;
                orangePortal.transform.parent = null;
            }


        }



        // Update is called once per frame
        void Update()
        {

        }
    }
}

