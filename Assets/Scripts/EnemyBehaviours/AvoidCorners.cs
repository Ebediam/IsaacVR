using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidCorners : EnemyBehaviour
{
    public float cornerAvoidDistance = 2f;
    public float accelerationMultiplier = 1f;
    List<Transform> corners;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!enemyController.active)
        {
            return;
        }
        base.Update();



        Vector3 direction = AvoidCornersDirection();

        if(direction == Vector3.zero)
        {
            return;
        }


        enemyController.rb.AddForce(direction * enemyController.data.acceleration*accelerationMultiplier, ForceMode.Acceleration);



    }

    public Vector3 AvoidCornersDirection()
    {
        Vector3 avoidCornersDirection = Vector3.zero;

        foreach(Transform corner in corners)
        {
            if(Vector3.Distance(corner.position, transform.position) <= cornerAvoidDistance)
            {
                avoidCornersDirection += (transform.position - corner.position);
            }
        }

        avoidCornersDirection = new Vector3(avoidCornersDirection.x, 0f, avoidCornersDirection.z);


        return avoidCornersDirection;
    }

    public override void Initialize()
    {
        base.Initialize();


        corners = enemyController.enemyManager.room.corners;
    }

}
