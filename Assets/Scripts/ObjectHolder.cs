using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    public enum SpawnerType
    {
        Defined,
        RandomPickup,
        RandomItem
    }

    public SpawnerType spawnerType;
    public ItemData spawnItem;
    public ItemPoolData itemPoolData;
    public GameObject heldItem;
    public Transform spawnPoint;

    
   

    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        switch (spawnerType)
        {
            case SpawnerType.Defined:
                if (!spawnItem)
                {
                    Debug.LogError("ObjectSpawner set to defined but no itemData found");
                    return;
                }
                break;

            case SpawnerType.RandomItem:
                spawnItem = itemPoolData.itemPool[Random.Range(0, itemPoolData.itemPool.Count)];
                break;

            case SpawnerType.RandomPickup:
                spawnItem = itemPoolData.pickupPool[Random.Range(0, itemPoolData.pickupPool.Count)];
                break;
        }



        heldItem = Instantiate(spawnItem.itemPrefab);
        heldItem.transform.position = spawnPoint.position;
        heldItem.transform.rotation = spawnPoint.rotation;
        heldItem.transform.parent = spawnPoint;
    }

    // Update is called once per frame
    void Update()
    {
        spawnPoint.transform.Rotate(spawnPoint.up, rotationSpeed*Time.deltaTime);
    }
}
