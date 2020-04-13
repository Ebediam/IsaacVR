using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Damageable
{
    public PoolData itemPoolData;
    public Transform spawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        DamageableDestroyedEvent += SpawnItem;
        currentHealth = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItem(Damageable damageable)
    {
        rb.detectCollisions = false;
        DamageableDestroyedEvent -= SpawnItem;
        GameObject randomPickup = Instantiate(itemPoolData.pickupPool[Random.Range(0, itemPoolData.pickupPool.Count)].prefab);
        randomPickup.transform.position = spawnPoint.position;
        randomPickup.transform.rotation = spawnPoint.rotation;

    }

}
