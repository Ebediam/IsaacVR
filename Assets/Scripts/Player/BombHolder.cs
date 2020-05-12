using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class BombHolder : Holder
    {
        public Bomb inRangeBomb;
        public GameObject bombIndicator;
        public Grabber inRangeHand;

        public void Start()
        {
            Grabber.FailGrabEvent += WithdrawBomb;

        }

        private void OnTriggerEnter(Collider other)
        {
            Bomb bomb = other.gameObject.GetComponentInParent<Bomb>();

            if (bomb)
            {
                bombIndicator.SetActive(true);
                inRangeBomb = bomb;
                inRangeBomb.OnItemDrop += StoreBomb;
                return;
            }


            /*
            Grabber grabber = other.gameObject.GetComponentInChildren<Grabber>();

            if (grabber)
            {
                if (!grabber.heldInteractable)
                {
                    inRangeHand = grabber;
                    bombIndicator.SetActive(true);
                }
            }
            */

        }

        private void OnTriggerExit(Collider other)
        {



            Bomb bomb = other.gameObject.GetComponentInParent<Bomb>();
            if (bomb)
            {
                if (bomb == inRangeBomb)
                {
                    inRangeBomb = null;
                    bombIndicator.SetActive(false);
                    return;
                }
            }

            /*
            Grabber hand = other.gameObject.GetComponentInChildren<Grabber>();

            if (hand)
            {
                if(hand == inRangeHand)
                {
                    inRangeHand = null;
                    bombIndicator.SetActive(false);
                    return;
                }
            }
            */

        }

        public void StoreBomb()
        {
            if (!inRangeBomb)
            {
                return;
            }

            if (inRangeBomb.activated)
            {
                return;
            }

            Player.local.AddBombs(1);
            inRangeBomb.DespawnItem();
            inRangeBomb = null;
            bombIndicator.SetActive(false);
        }

        public void WithdrawBomb(Grabber hand)
        {
            if (Player.local.data.bombs <= 0)
            {
                return;
            }

            if (hand.heldItem)
            {
                return;
            }

            if (Vector3.Distance(transform.position, hand.transform.position) > 0.35f)
            {
                return;
            }

            Item bombInstance = Item.SpawnItem(Player.local.data.bombData);
            hand.Grab(bombInstance);
            Player.local.AddBombs(-1);


        }
        private void Update()
        {

        }
    }
}

