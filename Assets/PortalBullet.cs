using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BOIVR
{

    public enum PortalColor
    {
        Orange,
        Blue
    }

    public class PortalBullet : AllBullet
    {
        public PortalColor color;

        // Start is called before the first frame update
        void Start()
        {
        }

        public override void CollisionEnterEvent(Collision collision)
        {
            base.CollisionEnterEvent(collision);

            rb.detectCollisions = false;
            rb.isKinematic = true;

            Portal portal;
            switch (color)
            {

                case PortalColor.Orange:
                    portal = PortalGun.orangePortal;
                    break;


                default:
                    portal = PortalGun.bluePortal;
                    break;

            }

            portal.transform.position = transform.position;
            portal.transform.LookAt(portal.transform.position + collision.GetContact(0).normal);
            portal.gameObject.SetActive(true);
            //gameObject.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}