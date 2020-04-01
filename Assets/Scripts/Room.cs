using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void EnemyKilled()
    {
        enemyCount--;

        if(enemyCount == 0)
        {
            Door.OpenDoors();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
