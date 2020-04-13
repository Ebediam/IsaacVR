using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public delegate void DungeonGeneratorDelegate();

    public DungeonGeneratorDelegate InitializeRoomsEvent;

    public RoomManager theVoidPosition;
    public DungeonData dungeonData;
    public RoomManager[,] positions;
    int generatedRooms = 0;
    int generatedBrances = 0;
    RoomManager startPosition;
    RoomManager itemPosition;
    RoomManager bossPosition;
    List<RoomManager> positionsWithRoom;
    bool movedPlayer = false;

    private void Awake()
    {
        positions = new RoomManager[dungeonData.maxColumns, dungeonData.maxRows];
        CreateGrid();
        positionsWithRoom = new List<RoomManager>();



        //startPosition = positions[Random.Range(0, dungeonData.maxColumns), Random.Range(0, dungeonData.maxRows)];
        startPosition = positions[Mathf.RoundToInt(dungeonData.maxRows) / 2, Mathf.RoundToInt(dungeonData.maxColumns / 2)];
        CreateRoom(startPosition);
        startPosition.room.roomType = Room.RoomType.Start;
        
        int iterations = 0;
        do
        {
            CreateBranch(positionsWithRoom[Random.Range(0,positionsWithRoom.Count)]);
            iterations++;


        } 
        while ((generatedRooms <= dungeonData.minRooms) && (iterations < 100));

        Debug.Log(iterations + " iterations to complete the dungeon");

        CreateBossRoom();
        CreatePowerupRoom();
        ConnectRooms();
        InitializeRoomsEvent?.Invoke();


        startPosition.room.isCompleted = true;

    }

    public void Start()
    {

    }

    public void Update()
    {
        if (!Player.local)
        {
            return;
        }

        if (movedPlayer)
        {
            return;
        }
        movedPlayer = true;
        Player.local.transform.position = startPosition.transform.position + Vector3.up * 0.5f;
    }

    public void CreateGrid()
    {
        for (int column = 0; column < dungeonData.maxColumns; column++)
        {
            for (int row = 0; row < dungeonData.maxRows; row++)
            {
                positions[row, column] = new GameObject().AddComponent<RoomManager>();
                positions[row, column].transform.parent = transform;
                positions[row, column].name = "Position " + column.ToString() + "-" + row.ToString();
                positions[row, column].transform.position = new Vector3(column * dungeonData.roomSide, 0f, row * dungeonData.roomSide);
                positions[row, column].column = column;
                positions[row, column].row = row;
                positions[row, column].zoneType = RoomManager.ZoneType.Empty;

                positions[row, column].partsData = dungeonData.dungeonParts;

                if(row > 0)
                {
                    positions[row, column].southPosition = positions[row-1, column];
                    positions[row-1, column].northPosition = positions[row, column];

                    if(row == (dungeonData.maxRows - 1))
                    {
                        positions[row, column].northPosition = theVoidPosition;
                    }

                }
                else
                {
                    positions[row, column].southPosition = theVoidPosition;
                }



                if(column > 0)
                {
                    positions[row, column].westPosition = positions[row, column-1];
                    positions[row, column-1].eastPosition = positions[row, column];

                    if(column == (dungeonData.maxColumns - 1))
                    {
                        positions[row, column].eastPosition = theVoidPosition;
                    }
                }
                else
                {
                    positions[row, column].westPosition = theVoidPosition;
                }


            }
        }



    }

    

    public void CreateBranch(RoomManager startingRoom)
    {
        RoomManager currentPosition = startingRoom;
        DirectionData lastDirection = RandomDirection();
        DirectionData randomDirection;

        generatedBrances++;
        Debug.Log("Iniciando rama " + generatedBrances + " en la habitacion " + currentPosition.room.number);
        int safetyCounter = 0;
        while (generatedRooms < dungeonData.maxRooms)
        {
            safetyCounter++;
            if(safetyCounter > dungeonData.maxBranchSize)
            {
                Debug.Log("Tamaño máximo de rama alcanzado con la habitación "+generatedRooms+", iniciando una nueva rama");
                return;
            }

            if(safetyCounter == 2)
            {
                randomDirection = lastDirection;
            }
            else
            {
                randomDirection = RandomDirection(dungeonData.directionLibrary.OpositeDirection(lastDirection));
            }

            //DirectionData randomDirection = RandomDirection();

            if (CheckAdjacentSpace(currentPosition, randomDirection, RoomManager.ZoneType.Empty) == true)
            {

                RoomManager validPosition = positions[currentPosition.row + randomDirection.rowsModifier, currentPosition.column + randomDirection.columnsModifier];
                CreateRoom(validPosition);
                lastDirection = randomDirection;
                currentPosition = validPosition;
            }
            else
            {
                Debug.Log("La rama numero "+generatedBrances+" ha chocado contra algo al intentar crear la habitacion "+(generatedRooms+1)+", generando rama nueva");
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
    
    public bool CheckAdjacentSpace(RoomManager startingRoom, DirectionData checkDirection, RoomManager.ZoneType typeOfRoomYouWantToCheck)
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

        if(positions[checkingRow, checkingColumn].zoneType == typeOfRoomYouWantToCheck)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void CreateRoom(RoomManager roomManager, string roomName)
    {
        roomManager.room = Instantiate(dungeonData.dungeonParts.room.GetComponentInChildren<Room>());
        roomManager.room.transform.parent = roomManager.transform;
        roomManager.room.transform.localPosition = Vector3.zero;
        roomManager.zoneType = RoomManager.ZoneType.Room;
        roomManager.room.doors = new List<Door>();
        generatedRooms++;
        roomManager.room.number = generatedRooms;
        positionsWithRoom.Add(roomManager);

        InitializeRoomsEvent += roomManager.InitializeRoom;
        roomManager.room.text.text = roomName;
        roomManager.room.roomType = Room.RoomType.Regular;

    }

    public void CreateRoom(RoomManager roomManager)
    {
        CreateRoom(roomManager, (generatedRooms+1).ToString());
    }

    public void ConnectRooms()
    {
        foreach(RoomManager roomManager in positionsWithRoom)
        {
            foreach(DirectionData direction in dungeonData.directionLibrary.directions)
            {
                if(CheckAdjacentSpace(roomManager, direction, RoomManager.ZoneType.Room))
                {
                    
                    GameObject doorFrame = Instantiate(dungeonData.dungeonParts.doorFrame);
                    doorFrame.transform.parent = roomManager.room.transform;
                    doorFrame.transform.localPosition = Vector3.zero;
                    doorFrame.transform.localRotation = Quaternion.identity;

                    doorFrame.transform.Rotate(doorFrame.transform.up, direction.angleToLookAt);

                    if((direction == dungeonData.directionLibrary.north)|| direction == dungeonData.directionLibrary.east)
                    {
                        Door door = Instantiate(dungeonData.dungeonParts.door).GetComponentInChildren<Door>();
                        door.rooms = new List<Room>();

                        door.transform.parent.parent = roomManager.transform;
                        door.transform.parent.localPosition = Vector3.zero;
                        door.transform.parent.localRotation = Quaternion.identity;

                        door.transform.parent.Rotate(door.transform.up, direction.angleToLookAt);

                        roomManager.room.doors.Add(door);

                        positions[roomManager.row+direction.rowsModifier, roomManager.column+direction.columnsModifier].room.doors.Add(door);




                    }
                }
                else
                {
                    
                    GameObject wall = Instantiate(dungeonData.dungeonParts.wall);
                    wall.transform.parent = roomManager.room.transform;
                    wall.transform.localPosition = Vector3.zero;
                    wall.transform.localRotation = Quaternion.identity;

                    wall.transform.Rotate(wall.transform.up, direction.angleToLookAt);
                }


            }


        }
    }

    public void CreateBossRoom()
    {
        for(int i= (positionsWithRoom.Count-1); i>=0; i--)
        {
            foreach(DirectionData direction in dungeonData.directionLibrary.directions)
            {
                if (!CheckAdjacentSpace(positionsWithRoom[i], direction, RoomManager.ZoneType.Empty))
                {
                    continue;
                }

                RoomManager checkingPosition = positions[(positionsWithRoom[i].row + direction.rowsModifier), (positionsWithRoom[i].column + direction.columnsModifier)];



                if (CheckIsolatedPosition(checkingPosition,1))
                {
                    foreach(DirectionData _direction in dungeonData.directionLibrary.directions)
                    {
                        if (!CheckAdjacentSpace(checkingPosition, direction, RoomManager.ZoneType.Empty))
                        {
                            continue;
                        }



                        RoomManager _checkingPosition = positions[(checkingPosition.row + direction.rowsModifier), (checkingPosition.column + direction.columnsModifier)];

                        if(CheckIsolatedPosition(_checkingPosition, 0))
                        {
                            CreateRoom(checkingPosition, "Boss");
                            bossPosition = checkingPosition;
                            bossPosition.room.roomType = Room.RoomType.Boss;

                            CreateRoom(_checkingPosition, "Teleporter");
                            _checkingPosition.room.roomType = Room.RoomType.Teleporter;

                            return;


                        }
                    }



                }
            }
        }
    }

    public void CreatePowerupRoom()
    {
        for (int i = (Mathf.RoundToInt(generatedRooms * 0.60f)); i >= 0; i--)
        {
            foreach (DirectionData direction in dungeonData.directionLibrary.directions)
            {
                if (!CheckAdjacentSpace(positionsWithRoom[i], direction, RoomManager.ZoneType.Empty))
                {
                    continue;
                }

                RoomManager checkingPosition = positions[(positionsWithRoom[i].row + direction.rowsModifier), (positionsWithRoom[i].column + direction.columnsModifier)];
                if (CheckIsolatedPosition(checkingPosition,1))
                {
                    CreateRoom(checkingPosition, "Item");
                    itemPosition = checkingPosition;
                    itemPosition.room.roomType = Room.RoomType.Treasure;
                    return;
                }
            }
        }
    }

    public bool CheckIsolatedPosition(RoomManager position, int maxAdjacentPositions)
    {
        if (position.room)
        {
            return false;
        }

        int adjacentRooms = 0;
        foreach(DirectionData direction in dungeonData.directionLibrary.directions)
        {
            if(CheckAdjacentSpace(position, direction, RoomManager.ZoneType.Room)){
                adjacentRooms++;
            }
        }

        if(adjacentRooms > maxAdjacentPositions)
        {
            return false;
        }
        else
        {
            return true;
        }


    }

}
