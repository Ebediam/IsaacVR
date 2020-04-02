using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public bool blocked;
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        if (blocked)
        {
            return;
        }

        Debug.Log(gameObject.name + " blocked by "+other.gameObject.name);

        blocked = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        blocked = false;
    }


}
