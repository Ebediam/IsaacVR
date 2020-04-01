using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public enum Side
    {
        Right,
        Left
    }
    public Side side;

    // Start is called before the first frame update
    void Start()
    {
        if(side == Side.Left)
        {
            GameManager.L1PressEvent += IndexTriggerPress;
            GameManager.L2PressEvent += HandTriggerPress;
        }
        else if(side == Side.Right)
        {
            GameManager.R1PressEvent += IndexTriggerPress;
            GameManager.R2PressEvent += HandTriggerPress;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IndexTriggerPress()
    {

    }

    void HandTriggerPress()
    {
        Grab();
    }

    void Grab()
    {

    }

}
