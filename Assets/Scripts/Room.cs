using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public delegate void RoomDelegate();

    public RoomDelegate RoomStartEvent;
    public RoomDelegate RoomEndEvent;

    public static Room activeRoom;

    public bool isCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RoomClear()
    {
        RoomEndEvent?.Invoke();
        Door.OpenDoors();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCompleted)
        {
            return;
        }

        if (other.gameObject.GetComponent<Player>())
        {
            RoomStartEvent?.Invoke();
        }
    }
}
