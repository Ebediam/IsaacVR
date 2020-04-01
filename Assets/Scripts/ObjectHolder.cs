using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    public ItemData spawnItem;
    public GameObject heldItem;
    public Transform spawnPoint;

    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
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
