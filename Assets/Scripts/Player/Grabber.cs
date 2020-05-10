using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public delegate void GrabberDelegate(Grabber grabber);
    public static GrabberDelegate FailGrabEvent;

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

    public InteractableType interactableType = InteractableType.None;

    public Side side;



    public List<Interactable> interactablesInRange;

    public Item heldItem;
    public Spell activeSpell;

    public Collider handCollider;
    public Rigidbody handRB;

    public Grabber otherHand;

    public static Grabber leftHand;
    public static Grabber rightHand;

    // Start is called before the first frame update
    void Start()
    {
        if(side == Side.Left)
        {
            GameManager.L1PressEvent += IndexTriggerPress;
            GameManager.L2PressEvent += HandTriggerPress;
            leftHand = this;


        }
        else if(side == Side.Right)
        {
            GameManager.R1PressEvent += IndexTriggerPress;
            GameManager.R2PressEvent += HandTriggerPress;
            GameManager.ButtonOnePressEvent += AltButtonPress;

            rightHand = this;
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
            leftHand = null;


        }
        else if (side == Side.Right)
        {
            GameManager.R1PressEvent -= IndexTriggerPress;
            GameManager.R2PressEvent -= HandTriggerPress;
            GameManager.ButtonOnePressEvent -= AltButtonPress;
            rightHand = null;
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


 
        }else if (activeSpell)
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
        if(buttonState == GameManager.ButtonState.Down)
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
       Interactable nearestInteractable =  Utils.CalculateNearestItem(interactablesInRange, transform.position);

        if (!nearestInteractable)
        {
            FailGrabEvent?.Invoke(this);
            return;
        }

        Grab(nearestInteractable);

        

    }

    public void Grab(Interactable interactable)
    {

        if(interactable is Item)
        {
            Item item = interactable as Item;
            handCollider.enabled = false;

            item.rb.constraints = RigidbodyConstraints.FreezeAll;


            item.transform.position = transform.position;
            item.transform.parent = transform;
            item.transform.localPosition -= item.holdPoint.localPosition;


            item.transform.rotation = transform.rotation;

            item.holder = this;
            heldItem = item;


            if (side == Side.Left)
            {
                Player.local.data.leftHandItem = item.data;
            }
            else if (side == Side.Right)
            {
                Player.local.data.rightHandItem = item.data;
            }

            item.OnItemPickup?.Invoke();

        }
        else if(interactable is Spell)
        {
            Spell spell = interactable as Spell;

            AddSpellAndActivate(spell, this);
            Spell _spell = Instantiate(spell).GetComponentInChildren<Spell>();
            AddSpellAndActivate(_spell, GetOtherHand());         

        }




    }

    public Grabber GetOtherHand()
    {

        switch (side)
        {

            case Side.Left:
                return rightHand;

            case Side.Right:
                return leftHand;


        }

        return null;


    }
    
    public static void AddSpellAndActivate(Spell spell, Grabber hand)
    {
        if(Player.local.data.availableSpells.Count > 0)
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

        if(heldItem)
        {
            heldItem.transform.parent = null;


            heldItem.rb.constraints = RigidbodyConstraints.None;
            heldItem.holder = null;

            heldItem.OnItemDrop?.Invoke();
            heldItem = null;

            if (side == Side.Left)
            {
                Player.local.data.leftHandItem = null;
            }
            else if (side == Side.Right)
            {
                Player.local.data.rightHandItem = null;
            }


            Invoke("ActivateHandCollider", 0.5f);
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


        if(interactablesInRange.Count > 0)
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

        if(interactablesInRange.Contains(interactable))
        {
            interactablesInRange.Remove(interactable);
        }
    }

    public static void RemoveFromItemsInRange(Interactable interactable)
    {
        if(leftHand.interactablesInRange.Count > 0)
        {
            if (leftHand.interactablesInRange.Contains(interactable))
            {
                leftHand.interactablesInRange.Remove(interactable);
            }
        }

        if(rightHand.interactablesInRange.Count > 0)
        {
            if (rightHand.interactablesInRange.Contains(interactable))
            {
                rightHand.interactablesInRange.Remove(interactable);
            }
        }


    }

}
