using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public delegate void GameManagerDelegate();
    public static GameManagerDelegate GameOverEvent;

    public delegate void ButtonPressDelegate();
    public delegate void JoystickPressDelegate(Vector2 vector2);
    public static ButtonPressDelegate L1PressEvent;
    public static ButtonPressDelegate L2PressEvent;
    public static ButtonPressDelegate R1PressEvent;
    public static ButtonPressDelegate R2PressEvent;

    public static JoystickPressDelegate leftJoystickEvent;
    public static JoystickPressDelegate rightJoystickEvent;

    public GameObject player;
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

        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch).magnitude > 0)
        {
            leftJoystickEvent(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch));
        }


        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).magnitude > 0)
        {
            rightJoystickEvent(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch));
        }

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
