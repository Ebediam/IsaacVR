using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;

namespace BOIVR
{

    public enum Direction
    {
        North,
        West,
        East,
        South,
        None
    }

    public enum PathfindDirection
    {
        None,
        Up,
        Down,
        Right,
        Left,
        Forward,
        Backwards
    }
    public static class Utils
    {

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


        public static Interactable CalculateNearestItem(List<Interactable> interactables, Vector3 centerPoint)
        {
            if (interactables.Count == 0)
            {
                return null;
            }


            Interactable nearestInteractable = null;

            float nearestDistance = 9999f;

            foreach (Interactable interactable in interactables)
            {
                if (!interactable)
                {
                    continue;
                }

                if (interactable.holder)
                {
                    continue;
                }

                float distance = Vector3.Distance(interactable.transform.position, centerPoint);
                if (distance < nearestDistance)
                {
                    nearestInteractable = interactable;
                    nearestDistance = distance;
                }
            }

            return nearestInteractable;
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

        public static void ChangeObjectLayer(GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach(Transform transform in gameObject.GetComponentsInChildren<Transform>())
            {
                transform.gameObject.layer = layer;
            }
        }
        public static Vector3 HorizontalVectorToPlayer(Vector3 startPosition)
        {
            Vector3 vectorToPlayer = Player.local.transform.position - startPosition;

            vectorToPlayer = new Vector3(vectorToPlayer.x, 0, vectorToPlayer.z);

            return vectorToPlayer;

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



        public static PathfindDirection RandomPathfindDirection(PathfindDirection lastDirection)
        {

            PathfindDirection direction = PathfindDirection.None;
            int random = Random.Range(0, 6);

            switch (random)
            {
                case 0:

                    if (lastDirection == PathfindDirection.Down)
                    {
                        direction = RandomPathfindDirection(lastDirection);
                    }
                    else
                    {
                        direction = PathfindDirection.Up;
                    }

                    break;

                case 1:

                    if (lastDirection == PathfindDirection.Up)
                    {
                        direction = RandomPathfindDirection(lastDirection);
                    }
                    else
                    {
                        direction = PathfindDirection.Down;
                    }

                    break;

                case 2:

                    if (lastDirection == PathfindDirection.Left)
                    {
                        direction = RandomPathfindDirection(lastDirection);
                    }
                    else
                    {
                        direction = PathfindDirection.Right;

                    }

                    break;

                case 3:

                    if (lastDirection == PathfindDirection.Right)
                    {
                        direction = RandomPathfindDirection(lastDirection);
                    }
                    else
                    {
                        direction = PathfindDirection.Left;
                    }
                    break;

                case 4:

                    if (lastDirection == PathfindDirection.Backwards)
                    {
                        direction = RandomPathfindDirection(lastDirection);
                    }
                    else
                    {
                        direction = PathfindDirection.Forward;
                    }

                    break;

                case 5:

                    if (lastDirection == PathfindDirection.Forward)
                    {
                        direction = RandomPathfindDirection(lastDirection);
                    }
                    else
                    {
                        direction = PathfindDirection.Backwards;
                    }

                    break;
            }

            return direction;
        }


        public static Vector3 VectorFromDirection(PathfindDirection direction)
        {
            Vector3 rdir = Vector3.zero;

            switch (direction)
            {
                case PathfindDirection.Up:

                    rdir = Vector3.up;
                    break;

                case PathfindDirection.Down:
                    
                    rdir = Vector3.up * -1f;  
                    break;

                case PathfindDirection.Right:

                    rdir = Vector3.right;      
                    break;

                case PathfindDirection.Left:
 
                    rdir = Vector3.right * -1f;                    
                    break;

                case PathfindDirection.Forward:


                    rdir = Vector3.forward;      
                    break;

                case PathfindDirection.Backwards:

                    rdir = Vector3.forward * -1f;
                    break;

            }

            return rdir;
        }

        public static void IgnoreCollisionsBetween(GameObject entity1, GameObject entity2)
        {
            foreach(Collider col in entity1.GetComponentsInChildren<Collider>())
            {
                foreach(Collider _col in entity2.GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(col, _col);
                }
            }
        }

    }

}