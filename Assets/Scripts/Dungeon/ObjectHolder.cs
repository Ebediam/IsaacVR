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

    [Header("Settings")]
    public SpawnerType spawnerType;
    public InteractableData spawnInteractable;
    public PoolData poolData;


    [Header("For debugging only")]
    public GameObject heldInteractable;
    public Transform spawnPoint;

    
   

    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        switch (spawnerType)
        {
            case SpawnerType.Defined:
                if (!spawnInteractable)
                {
                    Debug.LogError("ObjectSpawner set to defined but no itemData found");
                    return;
                }
                break;

            case SpawnerType.RandomItem:
                spawnInteractable = poolData.pool[Random.Range(0, poolData.pool.Count)];
                break;

            case SpawnerType.RandomPickup:
                spawnInteractable = poolData.pickupPool[Random.Range(0, poolData.pickupPool.Count)];
                break;
        }



        heldInteractable = Instantiate(spawnInteractable.prefab);
        heldInteractable.transform.position = spawnPoint.position;
        heldInteractable.transform.rotation = spawnPoint.rotation;
        heldInteractable.transform.parent = spawnPoint;
    }

    // Update is called once per frame
    void Update()
    {
        spawnPoint.transform.Rotate(spawnPoint.up, rotationSpeed*Time.deltaTime);
    }
}
