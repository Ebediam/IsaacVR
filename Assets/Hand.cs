using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Transform targetPosition;
    public Rigidbody rb;
    public float springForce;
    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = targetPosition.transform.position;
        rb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = targetPosition.rotation;

        rb.AddForce((targetPosition.position - transform.position) * springForce, ForceMode.Force);


    }
}
