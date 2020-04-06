using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DirectionData : ScriptableObject
{
    public enum DirectionType
    {
        Z_Axis,
        X_Axis
    }


    public int rowsModifier;
    public int columnsModifier;
    public float angleToLookAt;

    public DirectionType directionType;




}
