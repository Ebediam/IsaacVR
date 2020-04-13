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
    public GameObject keyLockedDoor;

    bool allClear = false;

    public List<Room> rooms;

    public enum DoorState
    {
        Open,
        Closed,
        KeyLocked,
        KeyLockedClosed
    }

    public bool hasPassed;
    public DoorState state;

    // Start is called before the first frame update
    void Start()
    {
        LockDoorsEvent += LockDoor;
        OpenDoorsEvent += OpenDoor;

        SetDoorState(state);
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bullet>())
        {
            other.gameObject.GetComponent<Bullet>().DestroyBullet();
        }

        if (other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<Player>().DeactivateGuns();
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != DoorState.KeyLocked)
        {
            return;
        }

        if (collision.gameObject.GetComponentInParent<Key>())
        {
            SetDoorState(DoorState.Open);
            collision.gameObject.GetComponentInParent<Key>().DespawnItem();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<Player>().ActivateGuns();
            hasPassed = true;

            if (allClear)
            {
                RemoveDoor();
            }
        }



    }

    public void OpenDoor()
    {          
        if(state == DoorState.Closed)
        {
            SetDoorState(DoorState.Open);

            allClear = true;
            foreach(Room room in rooms)
            {
                if (!room.isCompleted)
                {
                    allClear = false;
                }

            }

            if((allClear == true) && hasPassed)
            {
                RemoveDoor();
            }


        }
        else if(state == DoorState.KeyLockedClosed)
        {
            SetDoorState(DoorState.KeyLocked);
        }


    }

    public void RemoveDoor()
    {
        gameObject.SetActive(false);
    }

    public void LockDoor()
    {
        if(state == DoorState.Open)
        {
            SetDoorState(DoorState.Closed);
        }
        else if(state == DoorState.KeyLocked)
        {
            SetDoorState(DoorState.KeyLockedClosed);
        }
    }

    public static void OpenDoors()
    {
        OpenDoorsEvent?.Invoke();
    }
    public static void LockDoors()
    {
        LockDoorsEvent?.Invoke();
    } 

    public void SetDoorState(DoorState _state)
    {
        openDoor.SetActive(false);
        closedDoor.SetActive(false);
        keyLockedDoor.SetActive(false);
        state = _state;

        switch (_state)
        {
            case DoorState.Closed:
                
                closedDoor.SetActive(true);
                boxCollider.isTrigger = false;
                break;

            case DoorState.KeyLockedClosed:

                closedDoor.SetActive(true);
                boxCollider.isTrigger = false;
                break;

            case DoorState.KeyLocked:
                keyLockedDoor.SetActive(true);
                boxCollider.isTrigger = false;
                break;

            case DoorState.Open:
                openDoor.SetActive(true);
                boxCollider.isTrigger = true;
                break;

            default:
                Debug.LogError("Doorstate not valid");
                break;

        }
    }

}
