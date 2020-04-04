using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public delegate void DungeonGeneratorDelegate();

    public DungeonGeneratorDelegate InitializeRoomsEvent;


    public DungeonData dungeonData;
    public RoomManager[,] positions;
    int generatedRooms = 0;
    RoomManager startPosition;
    List<RoomManager> positionsWithRoom;

    private void Awake()
    {
        positions = new RoomManager[dungeonData.maxColumns, dungeonData.maxRows];
        CreateGrid();
        positionsWithRoom = new List<RoomManager>();


        startPosition = positions[Random.Range(0, dungeonData.maxColumns), Random.Range(0, dungeonData.maxRows)];
        CreateRoom(startPosition);

        int iterations = 0;
        do
        {
            CreateBranch(positionsWithRoom[Random.Range(0,positionsWithRoom.Count)]);
            iterations++;


        } 
        while ((generatedRooms < dungeonData.minRooms) || iterations > 100);

        Debug.Log(iterations + " iterations to complete the dungeon");

        InitializeRoomsEvent?.Invoke();
        ConnectRooms();

        startPosition.room.isCompleted = true;

    }

    public void Start()
    {
        Player.local.transform.position = startPosition.transform.position + Vector3.up * 0.5f;
    }


    public void CreateGrid()
    {
        for (int column = 0; column < dungeonData.maxColumns; column++)
        {
            for (int row = 0; row < dungeonData.maxRows; row++)
            {
                positions[column, row] = new GameObject().AddComponent<RoomManager>();
                positions[column, row].transform.parent = transform;
                positions[column, row].name = "Position " + column.ToString() + "-" + row.ToString();
                positions[column, row].transform.position = new Vector3(column * dungeonData.roomSide, 0f, row * dungeonData.roomSide);
                positions[column, row].column = column;
                positions[column, row].row = row;
                positions[column, row].roomType = RoomManager.RoomType.Empty;


            }
        }
    }

    public void CreateBranch(RoomManager startingRoom)
    {
        RoomManager currentPosition = startPosition;
        DirectionData lastDirection = RandomDirection();

        int safetyCounter = 0;
        while (generatedRooms < dungeonData.maxRooms)
        {
            safetyCounter++;
            if(safetyCounter > 100)
            {
                Debug.Log("Safety counter fired");
                return;
            }
            DirectionData randomDirection = RandomDirection(dungeonData.directionLibrary.OpositeDirection(lastDirection));

            if (CheckAdjacentSpace(currentPosition, randomDirection, RoomManager.RoomType.Empty))
            {

                RoomManager validPosition = positions[currentPosition.column + randomDirection.columnsModifier, currentPosition.row + randomDirection.rowsModifier];
                CreateRoom(validPosition);
                lastDirection = randomDirection;
                currentPosition = validPosition;
            }
            else
            {
                return;
            }
        }

        

    }

    public DirectionData RandomDirection()
    {
        DirectionData direction = dungeonData.directionLibrary.directions[Random.Range(0, 4)];

        return direction;
    }

    public DirectionData RandomDirection(DirectionData excludeDirection)
    {
        DirectionData direction;
        do
        {
            direction = RandomDirection();

        }
        while (direction == excludeDirection);
        
        return direction;
                
    }
    
    public bool CheckAdjacentSpace(RoomManager startingRoom, DirectionData checkDirection, RoomManager.RoomType typeOfRoomYouWantToCheck)
    {
        int checkingColumn = startingRoom.column + checkDirection.columnsModifier;

        if (checkingColumn > dungeonData.maxColumns - 1)
        {
            return false;
        }
        else if (checkingColumn < 0)
        {
            return false;
        }


        int checkingRow = startingRoom.row + checkDirection.rowsModifier;

        if (checkingRow > dungeonData.maxRows - 1)
        {
            return false;
        }
        else if (checkingRow < 0)
        {
            return false;
        }

        if(positions[checkingColumn, checkingRow].roomType == typeOfRoomYouWantToCheck)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void CreateRoom(RoomManager roomManager)
    {
        roomManager.room = Instantiate(dungeonData.dungeonParts.room.GetComponentInChildren<Room>());
        roomManager.room.transform.parent = roomManager.transform;
        roomManager.room.transform.localPosition = Vector3.zero;
        roomManager.roomType = RoomManager.RoomType.Room;
        generatedRooms++;
        positionsWithRoom.Add(roomManager);

        InitializeRoomsEvent += roomManager.InitializeRoom;

    }

    public void ConnectRooms()
    {
        foreach(RoomManager roomManager in positionsWithRoom)
        {
            foreach(DirectionData direction in dungeonData.directionLibrary.directions)
            {
                if(CheckAdjacentSpace(roomManager, direction, RoomManager.RoomType.Room))
                {
                    
                    GameObject doorFrame = Instantiate(dungeonData.dungeonParts.doorFrame);
                    doorFrame.transform.parent = roomManager.room.transform;
                    doorFrame.transform.localPosition = Vector3.zero;
                    doorFrame.transform.localRotation = Quaternion.identity;

                    doorFrame.transform.Rotate(doorFrame.transform.up, direction.angleToLookAt);

                    if((direction == dungeonData.directionLibrary.north)|| direction == dungeonData.directionLibrary.east)
                    {
                        Door door = Instantiate(dungeonData.dungeonParts.door).GetComponentInChildren<Door>();

                        door.transform.parent.parent = roomManager.transform;
                        door.transform.parent.localPosition = Vector3.zero;
                        door.transform.parent.localRotation = Quaternion.identity;

                        door.transform.parent.Rotate(door.transform.up, direction.angleToLookAt);

                        roomManager.room.doors.Add(door);
                        positions[roomManager.column+direction.columnsModifier, roomManager.row+direction.rowsModifier].room.doors.Add(door);


                    }
                }
                else
                {
                    //Poner wall
                    GameObject wall = Instantiate(dungeonData.dungeonParts.wall);
                    wall.transform.parent = roomManager.room.transform;
                    wall.transform.localPosition = Vector3.zero;
                    wall.transform.localRotation = Quaternion.identity;

                    wall.transform.Rotate(wall.transform.up, direction.angleToLookAt);
                }


            }


        }
    }

}
