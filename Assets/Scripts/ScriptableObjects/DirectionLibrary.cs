using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DirectionLibrary : ScriptableObject
{
    public List<DirectionData> directions;

    public DirectionData south;
    public DirectionData north;
    public DirectionData east;
    public DirectionData west;

    public DirectionData OpositeDirection(DirectionData direction)
    {
        if(direction == south)
        {
            return north;
        }
        else if(direction == north)
        {
            return south;
        }
        else if(direction == east)
        {
            return west;
        }
        else
        {
            return east;
        }

       
    }


}
