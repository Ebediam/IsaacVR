using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public enum Side
    {
        Right,
        Left
    }
    public Side side;

    public List<Item> itemsInRange;

    public Item heldItem;

    // Start is called before the first frame update
    void Start()
    {
        if(side == Side.Left)
        {
            GameManager.L1PressEvent += IndexTriggerPress;
            GameManager.L2PressEvent += HandTriggerPress;
        }
        else if(side == Side.Right)
        {
            GameManager.R1PressEvent += IndexTriggerPress;
            GameManager.R2PressEvent += HandTriggerPress;
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
            Grab();
        }

    }

    void Grab()
    {
       heldItem =  Utils.CalculateNearestItem(itemsInRange, transform.position);

        if (!heldItem)
        {
            return;
        }


        heldItem.rb.constraints = RigidbodyConstraints.FreezeAll;
        

        heldItem.transform.position = transform.position;        
        heldItem.transform.parent = transform;
        heldItem.transform.localPosition -= heldItem.holdPoint.localPosition;

        
        heldItem.transform.rotation = transform.rotation;


    }

    void Release()
    {
        heldItem.transform.parent = null;
        heldItem.rb.constraints = RigidbodyConstraints.None;
        heldItem = null;
    }

    private void OnTriggerEnter(Collider other)
    {
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

}
