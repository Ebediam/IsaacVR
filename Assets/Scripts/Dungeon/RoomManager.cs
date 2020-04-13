using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    public DungeonPartsData partsData;
    GameObject roomContent;
    public RoomManager northPosition;
    public RoomManager eastPosition;
    public RoomManager southPosition;
    public RoomManager westPosition;

    public List<GameObject> availableRoomContent;

    float rotation = 0f;

    public enum ZoneType
    {
        Empty,
        Room,
        TheVoid
    }



    public ZoneType zoneType;

    public int column;
    public int row;
    public Room room;


    public void InitializeRoom()
    {
        availableRoomContent = new List<GameObject>();

        AsignRoomContentList();

        switch (room.roomType)
        {
            case Room.RoomType.Regular:
                roomContent = availableRoomContent[Random.Range(0, availableRoomContent.Count)];
                break;

            case Room.RoomType.Boss:
                roomContent = partsData.bossRoomContentList[Random.Range(0, partsData.bossRoomContentList.Count)];
                break;

            case Room.RoomType.Treasure:
                roomContent = partsData.treasureRoomContentList[Random.Range(0, partsData.treasureRoomContentList.Count)];
                break;

            case Room.RoomType.Start:
                roomContent = partsData.firstRoomContent;
                break;

            case Room.RoomType.Teleporter:
                roomContent = partsData.teleporterRoomContent;
                break;


            default:
                return;


        }

        roomContent = Instantiate(roomContent);
        roomContent.transform.parent = room.transform;
        roomContent.transform.localPosition = Vector3.zero;
        roomContent.transform.localRotation = Quaternion.identity;
        roomContent.transform.Rotate(transform.up, rotation);

        
    }

    public void AsignRoomContentList()
    {
        if (northPosition.zoneType == ZoneType.Room)
        {
            if (eastPosition.zoneType == ZoneType.Room)
            {
                if (southPosition.zoneType == ZoneType.Room)
                {
                    if (westPosition.zoneType == ZoneType.Room)
                    {
                        //4-way room
                        availableRoomContent.AddRange(partsData.fourWayRoomContent);

                    }
                    else
                    {
                        //3-way no west
                        availableRoomContent.AddRange(partsData.threeWayRoomContent);
                        availableRoomContent.AddRange(partsData.fourWayRoomContent);
                        
                    }
                }
                else
                {
                    if (westPosition.zoneType == ZoneType.Room)
                    {
                        //3-way no south
                        availableRoomContent.AddRange(partsData.threeWayRoomContent);
                        availableRoomContent.AddRange(partsData.fourWayRoomContent);
                        rotation = 270f;

                    }
                    else
                    {
                        //2-way NE
                        availableRoomContent.AddRange(partsData.threeWayRoomContent);
                        availableRoomContent.AddRange(partsData.fourWayRoomContent);
                        availableRoomContent.AddRange(partsData.twoWayRoomContentTurn);
                    }
                }
            }
            else // Yes north, no east
            {
                if (southPosition.zoneType == ZoneType.Room)
                {
                    if (westPosition.zoneType == ZoneType.Room)
                    {
                        //3-way no east
                        availableRoomContent.AddRange(partsData.threeWayRoomContent);
                        availableRoomContent.AddRange(partsData.fourWayRoomContent);
                        rotation = 180f;
                    }
                    else
                    {
                        //2-way NS
                        availableRoomContent.AddRange(partsData.threeWayRoomContent);
                        availableRoomContent.AddRange(partsData.fourWayRoomContent);
                        availableRoomContent.AddRange(partsData.twoWayRoomContentStraigh);

                    }
                }
                else //Yes north, no east, no south
                {
                    if (westPosition.zoneType == ZoneType.Room)
                    {
                        //2-way NW
                        availableRoomContent.AddRange(partsData.threeWayRoomContent);
                        availableRoomContent.AddRange(partsData.fourWayRoomContent);
                        availableRoomContent.AddRange(partsData.twoWayRoomContentTurn);
                        rotation = 270f;
                    }
                    else
                    {
                        //1-way North
                        availableRoomContent.AddRange(partsData.oneWayRoomContent);
                    }
                }
            }


        }
        else //No north
        {
            if (eastPosition.zoneType == ZoneType.Room)
            {
                if (southPosition.zoneType == ZoneType.Room)
                {
                    if (westPosition.zoneType == ZoneType.Room)
                    {
                        //3-way no north
                        availableRoomContent.AddRange(partsData.threeWayRoomContent);
                        availableRoomContent.AddRange(partsData.fourWayRoomContent);
                        rotation = 90f;

                    }
                    else
                    {
                        //2-way ES
                        availableRoomContent.AddRange(partsData.threeWayRoomContent);
                        availableRoomContent.AddRange(partsData.fourWayRoomContent);
                        availableRoomContent.AddRange(partsData.twoWayRoomContentTurn);
                        rotation = 90f;
                    }
                }
                else //No north, no south, yes east
                {
                    if (westPosition.zoneType == ZoneType.Room)
                    {
                        //2-way EW
                        availableRoomContent.AddRange(partsData.threeWayRoomContent);
                        availableRoomContent.AddRange(partsData.fourWayRoomContent);
                        availableRoomContent.AddRange(partsData.twoWayRoomContentStraigh);
                        rotation = 90f;

                    }
                    else
                    {
                        //1-way east
                        availableRoomContent.AddRange(partsData.oneWayRoomContent);
                        rotation = 90f;

                    }
                }
            }
            else //No north, no east
            {
                if (southPosition.zoneType == ZoneType.Room)
                {
                    if (westPosition.zoneType == ZoneType.Room)
                    {
                        //2-way SW
                        availableRoomContent.AddRange(partsData.threeWayRoomContent);
                        availableRoomContent.AddRange(partsData.fourWayRoomContent);
                        availableRoomContent.AddRange(partsData.twoWayRoomContentTurn);
                        rotation = 180f;
                    }
                    else
                    {
                        //1-way south
                        availableRoomContent.AddRange(partsData.oneWayRoomContent);
                        rotation = 180f;

                    }
                }
                else //No north, no east, no south
                {
                    if (westPosition.zoneType == ZoneType.Room)
                    {
                        //1-way west
                        availableRoomContent.AddRange(partsData.oneWayRoomContent);
                        rotation = 270f;

                    }
                    else
                    {
                        Debug.LogError("Room has spawned with no adjacent rooms!");
                    }
                }
            }

        }
    }
}
