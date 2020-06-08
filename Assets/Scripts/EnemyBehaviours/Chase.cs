using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BOIVR
{



    public class Chase : EnemyBehaviour
    {
        public LayerMask groundLayer = 14;
        Transform target;
        PathfindDirection direction = PathfindDirection.None;
        PathfindDirection lastDirection = PathfindDirection.None;
        bool pathfinding = false;
        public float distanceThreshold = 0.5f;
        public float pathfindingStep = 1f;
        float distance;
        GameObject pathfindTarget;
        public int maxIterations = 5;
        int iterations = 0;
        public bool allowPathfind = false;
        Vector3 playerDirection;
        // Update is called once per frame
        public override void Action()
        {
            


            transform.LookAt(target);
            enemyController.rb.AddRelativeForce(Vector3.forward * enemyController.data.acceleration, ForceMode.Acceleration);
            if (Physics.Raycast(transform.position, transform.forward, pathfindingStep, groundLayer) && allowPathfind)
            {
                iterations = 0;
                pathfinding = true;
                Pathfind();
            }
     

            if (pathfinding)
            {
                playerDirection = enemyController.target.position - transform.position;

                if(Physics.Raycast(transform.position, playerDirection, playerDirection.magnitude, groundLayer))
                {
                    distance = Vector3.Distance(transform.position, target.position);
                    Debug.Log("Distance to target: " + distance);
                    if (distance < distanceThreshold)
                    {
                        EndPathfinding();
                    }
                }
                else
                {
                    EndPathfinding();
                }
            }

        }

        public void EndPathfinding()
        {
            pathfinding = false;
            target = enemyController.target;
            lastDirection = PathfindDirection.None;
        }

        public void Pathfind()
        {
            if(iterations > maxIterations)
            {
                Debug.Log("Max iterations exceeded, exiting pathfinding mode");
                target = enemyController.target;
                //pathfinding = false;
                return;
            }
            iterations++;
            Debug.Log("Chase behaviour found an obstacle, entering pathfinding mode");
            direction = Utils.RandomPathfindDirection(lastDirection);
            
            Vector3 checkDirection = Utils.VectorFromDirection(direction);
            if(Physics.Raycast(transform.position, checkDirection, pathfindingStep, groundLayer))
            {
                Debug.Log("Direction considered was blocked, trying again");
                lastDirection = direction;
                Pathfind();
            }
            else
            {
                pathfindTarget.transform.position = transform.position + checkDirection;
                target = pathfindTarget.transform;
            }

            


        }

        public override void Initialize()
        {
            base.Initialize();
            target = enemyController.target;
            pathfindTarget = new GameObject();
        }


    }
}

