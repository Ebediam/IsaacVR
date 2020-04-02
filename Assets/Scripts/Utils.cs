using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static Vector3 randomVector3()
    {
        Vector3 vector = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

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
}
