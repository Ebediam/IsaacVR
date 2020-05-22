using System.Collections;
using System.Collections.Generic;
using BOIVR;
using UnityEngine;

public class Arm : MonoBehaviour
{

    public Transform target;
    public bool active;
    public Hand hand;

    // Start is called before the first frame update
    void Start()
    {
        if (!hand.player.data.showArms)
        {
            gameObject.SetActive(false);
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            transform.LookAt(target);
        }


    }
}
