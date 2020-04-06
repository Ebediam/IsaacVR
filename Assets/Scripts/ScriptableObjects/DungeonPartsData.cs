using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DungeonPartsData : ScriptableObject
{
    [Header("Room parts")]
    public GameObject room;
    public GameObject doorFrame;
    public GameObject wall;
    public GameObject door;


    [Header("Special room content")]
    public GameObject firstRoomContent;
    public List<GameObject> treasureRoomContentList;
    public List<GameObject> bossRoomContentList;
    public GameObject teleporterRoomContent;

    [Header("Room content list")]
    public List<GameObject> oneWayRoomContent;
    public List<GameObject> twoWayRoomContentStraigh;
    public List<GameObject> twoWayRoomContentTurn;
    public List<GameObject> threeWayRoomContent;
    public List<GameObject> fourWayRoomContent;

}
