using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : EnemyBehaviour
{

    public Transform target;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GetTarget();
    }

    // Update is called once per frame
    public override void Update()
    {

        base.Update();

        if (!enemyController.active)
        {
            return;
        }

        if (!target)
        {
            GetTarget();
            return;
        }

        transform.LookAt(target);
    }

    public void GetTarget()
    {
        if (!Player.local)
        {
            return;
        }

        target = Player.local.head;
    }
}
