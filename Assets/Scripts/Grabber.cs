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
    public Side side;

    public List<Item> itemsInRange;

    public Item heldItem;

    public Collider handCollider;
    public Rigidbody handRB;


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
            rightHand = this;
        }

        itemsInRange = new List<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IndexTriggerPress()
    {
        if (heldItem)
        {
            heldItem.Use();
        }
    }

    void HandTriggerPress()
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

    void TryGrab()
    {
       Item nearestItem =  Utils.CalculateNearestItem(itemsInRange, transform.position);

        if (!nearestItem)
        {
            FailGrabEvent?.Invoke(this);
            return;
        }

        Grab(nearestItem);

        

    }

    public void Grab(Item item)
    {
        heldItem = item;
        handCollider.enabled = false;

        heldItem.rb.constraints = RigidbodyConstraints.FreezeAll;


        heldItem.transform.position = transform.position;
        heldItem.transform.parent = transform;
        heldItem.transform.localPosition -= heldItem.holdPoint.localPosition;





        heldItem.transform.rotation = transform.rotation;

        heldItem.holder = this;

        heldItem.OnItemPickup?.Invoke();
    }

    public void Release()
    {
        //heldItem.transform.parent = Room.activeRoom.transform ;
        heldItem.transform.parent = null;


        heldItem.rb.constraints = RigidbodyConstraints.None;
        heldItem.holder = null;

        heldItem.OnItemDrop?.Invoke();
        heldItem = null;


        Invoke("ActivateHandCollider", 0.5f);
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

        Item item = other.gameObject.GetComponentInParent<Item>();

        if (!item)
        {
            return;
        }


        if(itemsInRange.Count > 0)
        {
            if (itemsInRange.Contains(item))
            {
                return;
            }
        }

        itemsInRange.Add(item);
        
    }

    private void OnTriggerExit(Collider other)
    {
        Item item = other.gameObject.GetComponentInParent<Item>();

        if (itemsInRange.Count == 0 || !item)
        {
            return;
        }

        if(itemsInRange.Contains(item))
        {
            itemsInRange.Remove(item);
        }
    }

    public static void RemoveFromItemsInRange(Item item)
    {
        if(leftHand.itemsInRange.Count > 0)
        {
            if (leftHand.itemsInRange.Contains(item))
            {
                leftHand.itemsInRange.Remove(item);
            }
        }

        if(rightHand.itemsInRange.Count > 0)
        {
            if (rightHand.itemsInRange.Contains(item))
            {
                rightHand.itemsInRange.Remove(item);
            }
        }


    }

}
