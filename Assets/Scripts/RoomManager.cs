using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public enum RoomType
    {
        Empty,
        Room,
        StartRoom,
        MinibossRoom,
        PowerUpRoom,
        BossRoom,
        EndRoom
    }

    public RoomType roomType;

    public int column;
    public int row;
    public Room room;


    public void InitializeRoom()
    {
        room.doors = new List<Door>();

        
    }
}
