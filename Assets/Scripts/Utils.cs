using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    North,
    West,
    East,
    South,
    None
}

public class Utils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector3 RandomVector3()
    {
        Vector3 vector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        vector = vector.normalized;

        return vector;
    }
    
    public static Vector3 RandomVector3(bool zeroXaxis, bool zeroYaxis, bool zeroZaxis)
    {
        Vector3 vector = RandomVector3();

        if (zeroXaxis)
        {
            
        }

        if (zeroYaxis)
        {
            vector = new Vector3(vector.x, 0f, vector.z);
        }

        if (zeroZaxis)
        {
            vector = new Vector3(vector.x, vector.y, 0f);
        }

        vector = vector.normalized;

        return vector;
    }


    public static Item CalculateNearestItem(List<Item> items, Vector3 centerPoint)
    {
        if(items.Count == 0)
        {
            return null;
        }


        Item nearestItem = null;

        float nearestDistance = 9999f;

        foreach(Item item in items)
        {
            if (item.holder)
            {
                continue;
            }

            float distance = Vector3.Distance(item.transform.position, centerPoint);
            if (distance < nearestDistance)
            {
                nearestItem = item;
                nearestDistance = distance;
            }
        }

        return nearestItem;
    }

    public static Transform GetTarget()
    {
        Transform target = null;

        if (Player.local)
        {
           target = Player.local.head;
        }

        return target;


    } 

    public static Direction OpositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.East:
                return Direction.West;

            case Direction.North:
                return Direction.South;

            case Direction.South:
                return Direction.North;

            case Direction.West:
                return Direction.East;

            default:
                return Direction.None;

        }


    }

    public static Direction RandomDirection(Direction directionToExclude)
    {
        Direction direction;

        do
        {
            int randomNumber = Random.Range(0, 4);

            switch (randomNumber)
            {
                case 0:
                    direction = Direction.North;
                    break;

                case 1:
                    direction = Direction.West;
                    break;

                case 2:
                    direction = Direction.East;
                    break;

                case 3:
                    direction = Direction.South;
                    break;

                default:
                    Debug.LogError("Error generation random direction");
                    direction = Direction.South;
                    break;

            }
        } while (direction == directionToExclude);


        return direction;


       




    }
}
