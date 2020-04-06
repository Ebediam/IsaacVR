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
    public float maxHealth;
    float timer =0f;

    bool invincible = false;


    // Start is called before the first frame update
    void Start()
    {

        local = this;
        GameManager.GameOverEvent += OnGameOver;
        GameManager.leftJoystickEvent += Move;
        GameManager.rightJoystickEvent += Rotate;

        data.ClearItems();
        data.ClearModifiers();
        data.completedLevel = false;

        health = data.currentHealth;

        maxSpeed = data.maxSpeed;
        acceleration = data.acceleration;
        turnAngle = data.turnAngle;
        maxHealth = data.baseHealth+data.healthBoost;



        PlayerTookDamageEvent += UpdateHealth;
        UpdateHealth();

        if (data.leftHandItem)
        {
            Item leftItem = Item.SpawnItem(data.leftHandItem);
            Grabber.leftHand.Grab(leftItem);
        }


        if (data.rightHandItem)
        {
            Item rightItem = Item.SpawnItem(data.rightHandItem);
            Grabber.rightHand.Grab(rightItem);
        }


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
        if (invincible)
        {
            return;
        }


        health -= damage;
        

        if (health <= 0f)
        {
            GameManager.GameOver();
            Player.local = null;
        }

        invincible = true;

        PlayerTookDamageEvent?.Invoke(damage);
    }

    static void UpdateHealth(float damage)
    {
        local.data.currentHealth = local.health;
        local.maxHealth = local.data.baseHealth + local.data.healthBoost;
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

    public void AddCoins(int number)
    {
        data.coins += number;
        UpdateInventoryEvent?.Invoke();
    }

    public void OnGameOver()
    {
        if (data.completedLevel)
        {
            data.currentHealth = health;
        }

        GameManager.leftJoystickEvent -= Move;
        GameManager.rightJoystickEvent -= Rotate;
        PlayerTookDamageEvent -= UpdateHealth;
        GameManager.GameOverEvent -= OnGameOver;
        local = null;
    }

}
