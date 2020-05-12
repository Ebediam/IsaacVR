using System.Collections;
using System.Collections.Generic;
using BOIVR;
using UnityEngine;

namespace BOIVR
{
    public class TeleGrabber : MonoBehaviour
    {
        public Item teleItem;
        public Side side;
        public Hand hand;
        public bool held;
        public float springForce;
        public float damper;
        float oldDrag;


        // Start is called before the first frame update
        void Start()
        {

            switch (side)
            {
                case Side.Left:

                    GameManager.L2PressEvent += OnButtonPress;


                    break;

                default:
                    GameManager.R2PressEvent += OnButtonPress;
                    break;
            }


            GameManager.GameOverEvent += OnGameOver;

        }

        // Update is called once per frame
        void Update()
        {



        }

        public void OnButtonPress(GameManager.ButtonState buttonState)
        {
            if(buttonState == GameManager.ButtonState.Up)
            {
                if (held)
                {
                    StopTelegrab();
                }
            }
            else if(buttonState == GameManager.ButtonState.Down)
            {
                TryTelegrab();
                
            }


        }

        public void TryTelegrab()
        {
            if (!held)
            {
                if (teleItem)
                {
                    if (!teleItem.holder)
                    {
                        foreach(Interactable interactable in hand.grabber.interactablesInRange)
                        {
                            if (interactable is Item)
                            {
                                Item _item = interactable as Item;
                                if (teleItem == _item)
                                {
                                    return;
                                }
                            }
                        }

                       

                        StartTelegrab(teleItem);
                    }


                }
            }
        }

        public void FixedUpdate()
        {
            if (held)
            {
                if (teleItem)
                {
                    if (teleItem.holder)
                    {
                        StopTelegrab();
                    }
                    else
                    {
                        teleItem.rb.AddForce((transform.position - teleItem.transform.position) * springForce, ForceMode.Force);
                        teleItem.transform.rotation = hand.transform.rotation;
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (held)
            {
                return;
            }

            Item item = other.GetComponentInParent<Item>();

            if (item)
            {
                if (teleItem)
                {
                    if(Vector3.Distance(item.transform.position, hand.transform.position) > Vector3.Distance(teleItem.transform.position, hand.transform.position))
                    {
                        
                    }
                    else
                    {
                        teleItem = item;
                    }



                }
                else
                {
                    teleItem = item;
                }
            }



        }

        public void StartTelegrab(Item item)
        {
            held = true;
            oldDrag = item.rb.drag;
            item.rb.useGravity = false;
            item.rb.drag = damper;
            item.transform.parent = null;
            
        }

        public void StopTelegrab()
        {
            teleItem.rb.drag = oldDrag;
            held = false;
            teleItem.rb.useGravity = true;
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (held)
            {
                return;
            }


            Item item = other.GetComponentInParent<Item>();

            if (item)
            {
                if (item == teleItem)
                {
                    teleItem = null;
                }
            }

        }

        public void OnGameOver()
        {


            switch (side)
            {
                case Side.Left:

                    GameManager.L2PressEvent -= OnButtonPress;


                    break;

                default:
                    GameManager.R2PressEvent -= OnButtonPress;
                    break;
            }


            GameManager.GameOverEvent -= OnGameOver;
        }

    }


}

