using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class KeyHolder : Holder
    {
        public Key inRangeKey;
        public GameObject keyIndicator;
        public Grabber inRangeHand;

        public void Start()
        {
            Grabber.FailGrabEvent += WithdrawKey;
            GameManager.GameOverEvent += OnGameOver;
        }

        public void OnGameOver()
        {
            Grabber.FailGrabEvent -= WithdrawKey;
            GameManager.GameOverEvent -= OnGameOver;
        }

        private void OnTriggerEnter(Collider other)
        {
            Key key = other.gameObject.GetComponentInParent<Key>();

            if (key)
            {
                keyIndicator.SetActive(true);
                inRangeKey = key;
                inRangeKey.OnItemDrop += StoreKey;
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

            Key key = other.gameObject.GetComponentInParent<Key>();
            if (key)
            {
                if (key == inRangeKey)
                {
                    inRangeKey = null;
                    keyIndicator.SetActive(false);
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

        public void StoreKey()
        {
            if (!inRangeKey)
            {
                return;
            }

            Player.local.AddKeys(1);
            inRangeKey.DespawnItem();
            inRangeKey = null;
            keyIndicator.SetActive(false);
        }

        public void WithdrawKey(Grabber hand)
        {
            if (Player.local.data.keys <= 0)
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

            Item keyInstance = Item.SpawnItem(Player.local.data.keyData);
            hand.Grab(keyInstance);
            Player.local.AddKeys(-1);


        }
    }
}

