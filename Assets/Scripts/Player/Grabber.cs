﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public enum Side
    {
        Right,
        Left
    }

    public enum InteractableType
    {
        None,
        Item,
        Spell
    }
    public class Grabber : MonoBehaviour
    {
        public delegate void GrabberDelegate(Grabber grabber);
        public delegate void GrabberItemDelegate(Grabber grabber, Item item);
        public static GrabberDelegate FailGrabEvent;
        public static GrabberItemDelegate GrabEvent;
        public static GrabberItemDelegate ReleaseEvent;
        public static GrabberDelegate GetSpellEvent;

        public InteractableType interactableType = InteractableType.None;

        public Side side;

        float oldDrag;
        float oldAngularDrag;

        
        public List<Interactable> interactablesInRange;

        public Item heldItem;
        public Spell activeSpell;

        public Collider handCollider;
        public Hand hand;

        public Grabber otherGrabber;

        public static Grabber leftGrabber;
        public static Grabber rightGrabber;

        // Start is called before the first frame update
        void Start()
        {
            if (side == Side.Left)
            {
                GameManager.L1PressEvent += IndexTriggerPress;
                GameManager.L2PressEvent += HandTriggerPress;
                leftGrabber = this;

   
                if (hand.player.data.leftGrabberItem)
                {
                    Item leftItem = Item.SpawnItem(hand.player.data.leftGrabberItem);
                    leftItem.transform.position = transform.position + transform.forward;
                    Grab(leftItem);
                }

            }
            else if (side == Side.Right)
            {
                GameManager.R1PressEvent += IndexTriggerPress;
                GameManager.R2PressEvent += HandTriggerPress;
                GameManager.ButtonOnePressEvent += AltButtonPress;

                rightGrabber = this;

                if (hand.player.data.rightGrabberItem)
                {
                    Item rightItem = Item.SpawnItem(hand.player.data.rightGrabberItem);
                    rightItem.transform.position = transform.position + transform.forward;
                    Grab(rightItem);
                }

            }

            if (hand.player.data.activeSpell)
            {
                Spell spell = Instantiate(hand.player.data.activeSpell.prefab).GetComponent<Spell>();

                AddSpellAndActivate(spell, this);
            }

            interactablesInRange = new List<Interactable>();
            GameManager.GameOverEvent += OnGameOver;



        }

        public void OnGameOver()
        {
            if (side == Side.Left)
            {
                GameManager.L1PressEvent -= IndexTriggerPress;
                GameManager.L2PressEvent -= HandTriggerPress;
                leftGrabber = null;


            }
            else if (side == Side.Right)
            {
                GameManager.R1PressEvent -= IndexTriggerPress;
                GameManager.R2PressEvent -= HandTriggerPress;
                GameManager.ButtonOnePressEvent -= AltButtonPress;
                rightGrabber = null;
            }


            GameManager.GameOverEvent -= OnGameOver;
        }
        // Update is called once per frame
        void FixedUpdate()
        {

        }

        void IndexTriggerPress(GameManager.ButtonState buttonState)
        {
            if (heldItem)
            {
                switch (buttonState)
                {

                    case GameManager.ButtonState.Down:
                        heldItem.Use();
                        break;


                    case GameManager.ButtonState.Up:

                        heldItem.StopUsing();
                        break;


                }



            }
            else if (activeSpell)
            {
                switch (buttonState)
                {

                    case GameManager.ButtonState.Down:
                        activeSpell.Use();
                        break;


                    case GameManager.ButtonState.Up:

                        activeSpell.StopUsing();
                        break;


                }
            }
        }

        void HandTriggerPress(GameManager.ButtonState buttonState)
        {
            if (buttonState == GameManager.ButtonState.Down)
            {
                if (heldItem)
                {
                    Release();
                }
                else
                {
                    TryGrab();
                }
            }
        }

        void AltButtonPress(GameManager.ButtonState buttonState)
        {
            if (heldItem)
            {
                switch (buttonState)
                {
                    case GameManager.ButtonState.Down:
                        heldItem.AltUse();
                        break;

                    case GameManager.ButtonState.Up:

                        heldItem.StopAltUse();
                        break;
                }
            }
        }

        void TryGrab()
        {
            Interactable nearestInteractable = Utils.CalculateNearestItem(interactablesInRange, transform.position);

            if (!nearestInteractable)
            {
                FailGrabEvent?.Invoke(this);
                return;
            }

            Grab(nearestInteractable);



        }

        public void Grab(Interactable interactable)
        {
            if (interactable is Item)
            {
                heldItem = interactable as Item;

                DisableCollisions(heldItem);

                heldItem.rb.velocity = Vector3.zero;
                heldItem.rb.angularVelocity = Vector3.zero;
                


                heldItem.transform.position = transform.position;
                heldItem.transform.position -= heldItem.holdPoint.localPosition;
                heldItem.transform.rotation = transform.rotation;

                AddJoint(heldItem);
                heldItem.transform.parent = hand.player.transform;

                heldItem.holder = this;
                heldItem.rb.useGravity = false;


                if (side == Side.Left)
                {
                    hand.player.data.leftGrabberItem = heldItem.data;
                }
                else if (side == Side.Right)
                {
                    hand.player.data.rightGrabberItem = heldItem.data;
                }

                Utils.ChangeObjectLayer(heldItem.gameObject, 20);

                GrabEvent?.Invoke(this, heldItem);
                heldItem.OnItemPickup?.Invoke();

            }
            else if (interactable is Spell)
            {
                Spell spell = interactable as Spell;

                AddSpellAndActivate(spell, this);
                if (!GetOtherHand().activeSpell)
                {
                    Spell _spell = Instantiate(spell).GetComponentInChildren<Spell>();
                    AddSpellAndActivate(_spell, GetOtherHand());
                }


                GetSpellEvent?.Invoke(this);

            }




        }

        public void AddJoint(Item item)
        {
            item.AddJoint(this);
        }


        public void ChangeDrag()
        {
            oldDrag = heldItem.rb.drag;
            oldAngularDrag = heldItem.rb.angularDrag;
            heldItem.rb.drag = hand.player.data.grabDamper;
            heldItem.rb.angularDrag = hand.player.data.rotDamper;
        }
        public void DisableCollisions(Item item)
        {
            foreach (Collider col in item.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(col, handCollider, true);
            }
        }

        public Grabber GetOtherHand()
        {

            switch (side)
            {

                case Side.Left:
                    return rightGrabber;

                case Side.Right:
                    return leftGrabber;


            }

            return null;


        }

        public static void AddSpellAndActivate(Spell spell, Grabber grabber)
        {
            if (grabber.hand.player.data.availableSpells.Count > 0)
            {
                if (!grabber.hand.player.data.availableSpells.Contains(spell.data))
                {
                    grabber.hand.player.data.availableSpells.Add(spell.data);
                }
            }

            grabber.hand.player.data.activeSpell = spell.data;

            spell.OnSpellGrabbed(grabber);

            grabber.activeSpell = spell;

        }

        public void ResetDrag()
        {
            heldItem.rb.drag = oldDrag;
            heldItem.rb.angularDrag = oldAngularDrag;
        }


        public void ResetJoint(Item item)
        {

             item.RemoveJoint();
            
        }
        public void Release()
        {

            if (heldItem)
            {
                heldItem.transform.parent = null;

                ResetJoint(heldItem);

                //heldItem.rb.constraints = RigidbodyConstraints.None;
                heldItem.holder = null;
                heldItem.rb.useGravity = true;
                interactablesInRange.Remove(heldItem);
                heldItem.rb.velocity = hand.rb.velocity*2f;                
                heldItem.OnItemDrop?.Invoke();
                StartCoroutine(ReenableCollisions(heldItem, 0.5f));

                Utils.ChangeObjectLayer(heldItem.gameObject, 9);

                ReleaseEvent?.Invoke(this, heldItem);
                heldItem = null;

                if (side == Side.Left)
                {
                    hand.player.data.leftGrabberItem = null;
                }
                else if (side == Side.Right)
                {
                    hand.player.data.rightGrabberItem = null;
                }
                                
            }


        }

        public IEnumerator ReenableCollisions(Item item, float time)
        {
            yield return new WaitForSeconds(time);
            foreach (Collider col in item.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(col, handCollider, false);
            }


            

        }

        

        public void ActivateHandCollider()
        {           

            handCollider.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
            {
                return;
            }

            Interactable interactable = other.gameObject.GetComponentInParent<Interactable>();

            if (!interactable)
            {
                return;
            }


            if (interactablesInRange.Count > 0)
            {
                if (interactablesInRange.Contains(interactable))
                {
                    return;
                }
            }

            interactablesInRange.Add(interactable);

        }

        private void OnTriggerExit(Collider other)
        {
            Interactable interactable = other.gameObject.GetComponentInParent<Interactable>();

            if (interactablesInRange.Count == 0 || !interactable)
            {
                return;
            }

            if (interactablesInRange.Contains(interactable))
            {
                interactablesInRange.Remove(interactable);
            }
        }

        public static void RemoveFromItemsInRange(Interactable interactable)
        {
            if (leftGrabber.interactablesInRange.Count > 0)
            {
                if (leftGrabber.interactablesInRange.Contains(interactable))
                {
                    leftGrabber.interactablesInRange.Remove(interactable);
                }
            }

            if (rightGrabber.interactablesInRange.Count > 0)
            {
                if (rightGrabber.interactablesInRange.Contains(interactable))
                {
                    rightGrabber.interactablesInRange.Remove(interactable);
                }
            }


        }
    }


}
