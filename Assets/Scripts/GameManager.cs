using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public enum ButtonState
    {
        Down,
        Up
    }


    public delegate void GameManagerDelegate();
    public static GameManagerDelegate GameOverEvent;

    public delegate void ButtonPressDelegate(ButtonState buttonState);
    public delegate void JoystickPressDelegate(Vector2 vector2);
    public static ButtonPressDelegate L1PressEvent;
    public static ButtonPressDelegate L2PressEvent;
    public static ButtonPressDelegate R1PressEvent;
    public static ButtonPressDelegate R2PressEvent;
    public static ButtonPressDelegate RightThumbstickPressEvent;

    public static JoystickPressDelegate leftJoystickEvent;
    public static JoystickPressDelegate rightJoystickEvent;

    public GameObject player;



    bool currentThumbstickState;
    bool lastThumbstickState;

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

        if (!Player.local)
        {
            return;
        }

        // -------------------Left joystick

        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch).magnitude > 0)
        {
            leftJoystickEvent(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch));
        }


        // ------------------Right joystick

        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).magnitude > 0)
        {
            rightJoystickEvent(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch));
        }



        // ----------------Right Thumbstick press--------------

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch)) 
        {
            RightThumbstickPressEvent?.Invoke(ButtonState.Down);
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
        {
            RightThumbstickPressEvent?.Invoke(ButtonState.Up);
        }




        //--------------R2----------------

        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch)) 
        {
            R2PressEvent?.Invoke(ButtonState.Down);
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
        {
            R2PressEvent?.Invoke(ButtonState.Up);
        }

        //------------------L2

        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch)) //R2
        {
            L2PressEvent?.Invoke(ButtonState.Down);
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch))
        {
            L2PressEvent?.Invoke(ButtonState.Up);
        }


        //--------------------------L1

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            L1PressEvent?.Invoke(ButtonState.Down);

        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            L1PressEvent?.Invoke(ButtonState.Up);
        }

        //-----------------R1-----------------------
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)) 
        {
            R1PressEvent?.Invoke(ButtonState.Down);
            
        }
        else if(OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            R1PressEvent?.Invoke(ButtonState.Up);
        }


        



        if(OVRInput.GetDown(OVRInput.Button.Left, OVRInput.Controller.LTouch))
        {

        }


    }

    public static void GameOver()
    {
        GameOverEvent?.Invoke();
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
