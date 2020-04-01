using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        GetTarget();
    }

    // Update is called once per frame
    void Update()
    {
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
