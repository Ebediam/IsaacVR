using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerControls controls;
    public Rigidbody rb;
    public PlayerData data;
    public Transform groundCheck;

    public Transform view;

    float acceleration;
    float maxSpeed;
    float turnAngle;

    public float xSensitivity;
    public float ySensitivity;

    public bool isGrounded;
    bool canJump = true;
    Ray groundRay;

    Vector2 direction;
    Vector2 rotateDirection;

    float yRotation;
    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Jump.performed += Jump;
        controls.Gameplay.Move.performed += MoveMask;
        controls.Gameplay.Move.canceled += StopMoving;
        controls.Gameplay.Rotate.performed += RotateMask;
        controls.Gameplay.Rotate.canceled += StopRotating;

    }

    private void Start()
    {
        
        acceleration = data.acceleration;
        maxSpeed = data.maxSpeed;
        turnAngle = data.turnAngle;
    }

    public void RotateMask(InputAction.CallbackContext context)
    {
        rotateDirection = context.ReadValue<Vector2>();
    }

    public void StopRotating(InputAction.CallbackContext context)
    {
        rotateDirection = Vector2.zero;
    }

    void Update()
    {
        if (canJump)
        {
            groundRay = new Ray(groundCheck.transform.position, transform.up * -1f);
            Debug.DrawRay(groundCheck.transform.position, transform.up * 0.3f * -1f, Color.red);
            if (Physics.Raycast(groundRay, out RaycastHit hitInfo, 0.3f, data.groundLayer))
            {
                isGrounded = true;

            }
            else
            {
                isGrounded = false;
            }
        }

        if(direction != Vector2.zero)
        {
            Move(direction);
        }

        if (rotateDirection.x != 0)
        {
            Rotate(rotateDirection);
        }

        if(rotateDirection.y != 0)
        {
            Look(rotateDirection.y);
        }

    }


    public void MoveMask(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    public void StopMoving(InputAction.CallbackContext context)
    {
        direction = Vector2.zero;
    }

    public void Move(Vector2 direction2D)
    {
        Vector3 direction = new Vector3(direction2D.x, 0f, direction2D.y);

        direction *= acceleration;

        rb.AddRelativeForce(direction * Time.deltaTime, ForceMode.Acceleration);

        //maxSpeed, data.movementBoost

        Vector2 horizontalSpeed = new Vector2(rb.velocity.x, rb.velocity.z);

        if (horizontalSpeed.magnitude > (maxSpeed + data.movementBoost))
        {
            horizontalSpeed = Vector2.ClampMagnitude(horizontalSpeed, (maxSpeed + data.movementBoost));

            rb.velocity = new Vector3(horizontalSpeed.x, rb.velocity.y, horizontalSpeed.y);
        }


    }


    public void Rotate(Vector2 direction2D)
    {


        if (Mathf.Abs(direction2D.x) > 0.3f)
        {
            transform.Rotate(transform.up, turnAngle*xSensitivity * direction2D.x);
        }




    }

    public void Look(float angle)
    {
        yRotation += angle*ySensitivity;

        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        view.localRotation = Quaternion.Euler(-yRotation, 0, 0);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump called");

        if (isGrounded)
        {
            if (canJump)
            {
                rb.AddForce(transform.up * data.jumpForce, ForceMode.VelocityChange);
                isGrounded = false;
            }
        }


    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }


}
