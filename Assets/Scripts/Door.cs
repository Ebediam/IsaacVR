using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public delegate void DoorDelegate();

    public static DoorDelegate LockDoorsEvent;
    public static DoorDelegate OpenDoorsEvent;
    public BoxCollider boxCollider;

    public GameObject openDoor;
    public GameObject closedDoor;
    public bool hasPassed = false;

    // Start is called before the first frame update
    void Start()
    {
        LockDoorsEvent += LockDoor;
        OpenDoorsEvent += OpenDoor;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            hasPassed = true;
            LockDoors();
        }

    }

    public void OpenDoor()
    {
        if (hasPassed)
        {
            gameObject.SetActive(false);
            return;
        }

        boxCollider.isTrigger = true;
        openDoor.SetActive(true);
        closedDoor.SetActive(false);

    }
    public void LockDoor()
    {
        boxCollider.isTrigger = false;
        openDoor.SetActive(false);
        closedDoor.SetActive(true);


    }

    public static void OpenDoors()
    {
        OpenDoorsEvent?.Invoke();
    }
    public static void LockDoors()
    {
        LockDoorsEvent?.Invoke();
    } 

}
