using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

namespace BOIVR
{
    public class Holder : MonoBehaviour
    {
        public ItemData heldItemData;
        public GameObject indicator;

        public List<Grabber> inRangeGrabbers = new List<Grabber>();

        public delegate void HolderDelegate(ItemData data, int totalAmount);
        public static HolderDelegate HolderUpdate;

        public int count = 0;

        // Start is called before the first frame update
        void Start()
        {
            
            Grabber.FailGrabEvent += Withdraw;
            Grabber.ReleaseEvent += Store;
            GameManager.GameOverEvent -= OnGameOver;
        }

        // Update is called once per frame

        private void OnTriggerEnter(Collider other)
        {
            Hand hand = other.gameObject.GetComponent<Hand>();

            if (hand)
            {
                if (!inRangeGrabbers.Contains(hand.grabber))
                {
                    inRangeGrabbers.Add(hand.grabber);
                }

                indicator.SetActive(true);
            }


        }

        private void OnTriggerExit(Collider other)
        {
            Hand hand = other.gameObject.GetComponent<Hand>();

            if (hand)
            {
                if (inRangeGrabbers.Contains(hand.grabber))
                {
                    inRangeGrabbers.Remove(hand.grabber);
                }

                if(inRangeGrabbers.Count == 0)
                {
                    indicator.SetActive(false);
                }
            }
        }



        public void Withdraw(Grabber grabber)
        {
            if(count == 0)
            {
                return;
            }

            if (inRangeGrabbers.Contains(grabber))
            {
                Item itemInstance = Item.SpawnItem(heldItemData);
                itemInstance.gameObject.SetActive(true);
                itemInstance.transform.position = this.transform.position;
                itemInstance.transform.rotation = transform.rotation;
                grabber.Grab(itemInstance);
                count--;
                HolderUpdate?.Invoke(heldItemData, count);
            }
        }

        public void Store(Grabber grabber, Item item)
        {
            if (inRangeGrabbers.Contains(grabber))
            {
                if(item.data == heldItemData)
                {
                    item.DespawnItem();
                    count++;
                    HolderUpdate?.Invoke(heldItemData, count);
                }


            }
        }

        void OnGameOver()
        {

            Grabber.FailGrabEvent -= Withdraw;
            Grabber.ReleaseEvent -= Store;
            GameManager.GameOverEvent -= OnGameOver;
            
        }
    }
}

