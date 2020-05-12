using System.Collections;
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
        public static GrabberDelegate FailGrabEvent;



        public InteractableType interactableType = InteractableType.None;

        public Side side;



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


            }
            else if (side == Side.Right)
            {
                GameManager.R1PressEvent += IndexTriggerPress;
                GameManager.R2PressEvent += HandTriggerPress;
                GameManager.ButtonOnePressEvent += AltButtonPress;

                rightGrabber = this;
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
        void Update()
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
                Item item = interactable as Item;

                DisableCollisions(item);
                

                

                item.rb.velocity = Vector3.zero;
                item.rb.angularVelocity = Vector3.zero;
                item.rb.constraints = RigidbodyConstraints.FreezeAll;


                item.transform.position = transform.position;
                item.transform.parent = transform;
                item.transform.localPosition -= item.holdPoint.localPosition;


                item.transform.rotation = transform.rotation;

                item.holder = this;
                heldItem = item;


                if (side == Side.Left)
                {
                    Player.local.data.leftGrabberItem = item.data;
                }
                else if (side == Side.Right)
                {
                    Player.local.data.rightGrabberItem = item.data;
                }

                item.OnItemPickup?.Invoke();

            }
            else if (interactable is Spell)
            {
                Spell spell = interactable as Spell;

                AddSpellAndActivate(spell, this);
                Spell _spell = Instantiate(spell).GetComponentInChildren<Spell>();
                AddSpellAndActivate(_spell, GetOtherHand());

            }




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

        public static void AddSpellAndActivate(Spell spell, Grabber hand)
        {
            if (Player.local.data.availableSpells.Count > 0)
            {
                if (!Player.local.data.availableSpells.Contains(spell.data))
                {
                    Player.local.data.availableSpells.Add(spell.data);
                }
            }

            Player.local.data.activeSpell = spell.data;

            spell.holder = hand;
            spell.grabDetectionCollider.enabled = false;
            spell.transform.position = hand.transform.position;
            spell.transform.rotation = hand.transform.rotation;
            spell.transform.parent = hand.transform;
            hand.activeSpell = spell;
            spell.idleSpell.SetActive(false);




        }



        public void Release()
        {

            if (heldItem)
            {
                heldItem.transform.parent = null;


                heldItem.rb.constraints = RigidbodyConstraints.None;
                heldItem.holder = null;
                heldItem.rb.useGravity = true;
                heldItem.rb.velocity = hand.rb.velocity*2f;
                heldItem.OnItemDrop?.Invoke();
                StartCoroutine(ReenableCollisions(heldItem, 0.5f));

                
                heldItem = null;

                if (side == Side.Left)
                {
                    Player.local.data.leftGrabberItem = null;
                }
                else if (side == Side.Right)
                {
                    Player.local.data.rightGrabberItem = null;
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
