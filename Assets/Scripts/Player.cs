using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player local;
    public Transform head;
    // Start is called before the first frame update
    void Start()
    {
        local = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
