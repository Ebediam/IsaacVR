using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public enum RoomType
    {
        Empty,
        Room,
        StartRoom
    }

    public RoomType roomType;

    public int column;
    public int row;
    public Room room;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
