using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DungeonData : ScriptableObject
{
    [Header("Dungeon base settings")]
    public int maxRows;
    public int maxColumns;
    public int maxRooms;
    public int minRooms;
    public int maxBranchSize;

    [Header("Room settings")]
    public float roomSide;

    [Header("Data")]
    public DungeonPartsData dungeonParts;

    public DirectionLibrary directionLibrary;
    public EnemyPoolData enemyPoolData;

    public PoolData itemPoolData;

}
