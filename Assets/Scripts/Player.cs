using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static PlayerDelegate EnterSafeZoneEvent;
    public static PlayerDelegate ExitSafeZoneEvent;

    public delegate void PlayerFloatDelegate(float floatVariable);
    public PlayerFloatDelegate PlayerTookDamageEvent;
    public static PlayerDelegate UpdateHealthEvent;
    public static PlayerDelegate UpdateInventoryEvent;

    public HandUIManager UIManager;

    public static Player local;
    public Rigidbody rb;
    public Transform head;
    public PlayerData data;
    float maxSpeed;
    float acceleration;
    float turnAngle;
    public float health;
    float timer =0f;

    bool invincible = false;


    // Start is called before the first frame update
    void Start()
    {

        local = this;
        GameManager.leftJoystickEvent += Move;
        GameManager.rightJoystickEvent += Rotate;

        data.ClearModifiers();
        data.ClearItems();

        maxSpeed = data.maxSpeed;
        acceleration = data.acceleration;
        turnAngle = data.turnAngle;
        health = data.maxHealth;

        PlayerTookDamageEvent += UpdateHealth;
        UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (!invincible)
        {
            return;
        }

        timer += Time.deltaTime;

        if(timer > data.invincibilityTime)
        {
            invincible = false;
            timer = 0f;
        }
    }

    public void Move(Vector2 direction2D)
    {

        
        Vector3 direction = new Vector3 (direction2D.x, 0f, direction2D.y);

        direction *= acceleration;

        rb.AddRelativeForce(direction * Time.deltaTime, ForceMode.Acceleration);

        //maxSpeed, data.movementBoost

        Vector2 horizontalSpeed = new Vector2(rb.velocity.x, rb.velocity.z);

        if(horizontalSpeed.magnitude > (maxSpeed + data.movementBoost))
        {
            horizontalSpeed = Vector2.ClampMagnitude(horizontalSpeed, (maxSpeed + data.movementBoost));

            rb.velocity = new Vector3(horizontalSpeed.x, rb.velocity.y, horizontalSpeed.y);
        }

        

       
                
    }

    public void Rotate (Vector2 direction2D)
    {
        if(direction2D.x > 0f)
        {
            transform.Rotate(transform.up, turnAngle);
        }
        if(direction2D.x < 0f)
        {
            transform.Rotate(transform.up, -turnAngle);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        

        if (health <= 0f)
        {
            GameManager.GameOver();
        }

        invincible = true;

        PlayerTookDamageEvent?.Invoke(damage);
    }

    static void UpdateHealth(float damage)
    {
        UpdateHealthEvent?.Invoke();
    }

    public static void UpdateHealth()
    {
        UpdateHealth(0f);
    }
    public void DeactivateGuns()
    {
        EnterSafeZoneEvent?.Invoke();
    }

    public void ActivateGuns()
    {
        ExitSafeZoneEvent?.Invoke();
    }

    public void AddBombs(int number)
    {
        data.bombs += number;
        UpdateInventoryEvent?.Invoke();

    }

    public void AddKeys(int number)
    {
        data.keys += number;
        UpdateInventoryEvent?.Invoke();
    }


}
