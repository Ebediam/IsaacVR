using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{

    public class Portal : MonoBehaviour
    {


        public Portal otherPortal;
        public PortalColor color;
        public Camera portalCamera;
        public Player player;
        // Start is called before the first frame update
        void Start()
        {
            player = Player.local;
            if (color == PortalColor.Blue)
            {
                otherPortal = PortalGun.orangePortal;
            }
            else
            {
                otherPortal = PortalGun.bluePortal;
            }
        }

        // Update is called once per frame
        void Update()
        {

            Vector3 playerOffset = otherPortal.transform.position - player.transform.position;

        }
    }

}