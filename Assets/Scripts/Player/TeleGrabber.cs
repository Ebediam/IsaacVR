using System;
using System.Collections;
using System.Collections.Generic;
using BOIVR;
using OVR.OpenVR;
using UnityEngine;

namespace BOIVR
{
    public class TeleGrabber : MonoBehaviour
    {
        public Item teleItem;
        public Side side;
        public Hand hand;
        public bool held;
        [HideInInspector] public float springForce;
        [HideInInspector] public float damper;
        float oldDrag;
        public Transform telePoint;

        public float maxItemDistance;
        public float maxHandDistance;
        public float minHandDistance;
        public float itemGrabThreshold;



        public float storedHandRatio;
        public float storedItemRatio;
        
        public float handRatio;
        public float itemRatio;


        public float slope;
        public float forwardSlope;
        public float backwardSlope;


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

            

            RetrievePlayerSettings();

            slope = maxItemDistance / (maxHandDistance - minHandDistance);

        }

        public void RetrievePlayerSettings()
        {

            springForce = hand.player.data.springForce;
            damper = hand.player.data.damper;
            minHandDistance = hand.player.data.minHandDistance;
            maxHandDistance = hand.player.data.maxHandDistance;
            maxItemDistance = hand.player.data.maxItemDistance;
            itemGrabThreshold = hand.player.data.itemGrabTreshold;

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
                        
                        handRatio = Vector3.Distance(hand.transform.position, hand.shoulder.position);
                        itemRatio = Vector3.Distance(telePoint.position, hand.shoulder.position);
                        Debug.Log("handDistance: " + handRatio);
                        handRatio = Mathf.Clamp(handRatio, minHandDistance, maxHandDistance);
                                                

                        telePoint.position = hand.shoulder.transform.position + (hand.transform.position - hand.shoulder.position).normalized * StepFunction(handRatio);                       

                        teleItem.rb.AddForce((telePoint.position - teleItem.transform.position) * springForce, ForceMode.Force);
                        teleItem.transform.rotation = hand.transform.rotation;
                        
                        if(Mathf.Abs(handRatio - storedHandRatio) > 0.1f)
                        {
                            CalculateSlopes();
                        }


                        CheckForGrab();

                    }
                }
            }
        }

        public void CheckForGrab()
        {
            float distance = Vector3.Distance(teleItem.transform.position, hand.transform.position);

            if(distance < itemGrabThreshold)
            {
                hand.grabber.Grab(teleItem);
                StopTelegrab();

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
            float distance = Vector3.Distance(item.transform.position, hand.shoulder.transform.position);
            telePoint.position = hand.shoulder.transform.position + (hand.transform.position - hand.shoulder.position).normalized * distance;

            CalculateSlopes();
        }

        public void CalculateSlopes()
        {
            storedHandRatio = Vector3.Distance(hand.transform.position, hand.shoulder.position);
            storedHandRatio = Mathf.Clamp(storedHandRatio, minHandDistance, maxHandDistance);

            storedItemRatio = Vector3.Distance(telePoint.position, hand.shoulder.position);
            storedItemRatio = Mathf.Clamp(storedItemRatio, 0, maxItemDistance);


            if(storedHandRatio < 0.05f)
            {
                backwardSlope = slope;
            }
            else
            {
                backwardSlope = storedItemRatio / (storedHandRatio-minHandDistance);
            }
            

            if(Mathf.Abs(maxHandDistance - storedHandRatio) < 0.01f)
            {
                forwardSlope = slope;
            }
            else
            {
                forwardSlope = (maxItemDistance - storedItemRatio) / (maxHandDistance - storedHandRatio);
            }         

        }

        public float StepFunction(float x)
        {
            float y;

            if(x < storedHandRatio)
            {
                y = backwardSlope * (x-minHandDistance);
            }
            else
            {
                y = forwardSlope * (x - storedHandRatio) + storedItemRatio;
            }

            return y;

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

