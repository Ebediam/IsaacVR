using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public delegate void ButtonPressDelegate();
    public static ButtonPressDelegate L1PressEvent;
    public static ButtonPressDelegate L2PressEvent;
    public static ButtonPressDelegate R1PressEvent;
    public static ButtonPressDelegate R2PressEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // A & X = One
        // B & Y = Two
        // Tres rayas (izq) = Start

        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch)) //L2
        {
            L2PressEvent?.Invoke();
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch)) //R2
        {
            R2PressEvent?.Invoke();
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch)) //L1
        {
            L1PressEvent?.Invoke();
        }


        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)) //R1
        {
            R1PressEvent?.Invoke();
        }



    }
}
