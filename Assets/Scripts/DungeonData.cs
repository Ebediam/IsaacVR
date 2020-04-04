using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DungeonData : ScriptableObject
{

    public int maxRows;
    public int maxColumns;
    public int maxRooms;
    public int minRooms;

    public float roomSide;

    public DungeonPartsData dungeonParts;

    public DirectionLibrary directionLibrary;

}
