using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class Player : MonoBehaviour
    {
        public delegate void PlayerDelegate();
        public static PlayerDelegate EnterSafeZoneEvent;
        public static PlayerDelegate ExitSafeZoneEvent;

        public delegate void PlayerFloatDelegate(float floatVariable);
        public PlayerFloatDelegate PlayerTookDamageEvent;
        public static PlayerDelegate UpdateHealthEvent;
        public static PlayerDelegate UpdateInventoryEvent;

        public Collider bodyCollider;
        public Body body;
        public Camera headCamera;

        public HandUIManager UIManager;

        public Transform groundCheck;

        public static Player local;
        public Rigidbody rb;
        public Transform head;
        public PlayerData data;
        float maxSpeed;
        float acceleration;
        float turnAngle;
        public float health;
        public float maxHealth;

        public float mana;
        public float maxMana;

        float timer = 0f;

        float jumpTimer;
        float jumpCooldown;

        bool invincible = false;
        bool canJump;
        public bool flying;
        public bool isGrounded;
        Ray groundRay;
        // Start is called before the first frame update
        void Start()
        {
            local = this;
            GameManager.GameOverEvent += OnGameOver;
            GameManager.leftJoystickEvent += Move;
            GameManager.rightJoystickEvent += Rotate;
            GameManager.RightThumbstickPressEvent += Jump;

            data.ClearItems();
            data.ClearModifiers();
            data.ClearSpells();
            data.completedLevel = false;

            health = data.currentHealth;



            maxSpeed = data.maxSpeed;
            acceleration = data.acceleration;
            turnAngle = data.turnAngle;
            maxHealth = data.baseHealth + data.healthBoost;



            PlayerTookDamageEvent += UpdateHealth;
            Holder.HolderUpdate += HolderUpdater;


            UpdateHealth();
        }

        // Update is called once per frame
        void Update()
        {
            if (canJump)
            {
                groundRay = new Ray(groundCheck.transform.position, transform.up * -1f);
                Debug.DrawRay(groundCheck.transform.position, transform.up * 0.3f * -1f, Color.red);
                if (Physics.Raycast(groundRay, out RaycastHit hitInfo, 0.3f, data.groundLayer))
                {
                    isGrounded = true;
                    if (flying)
                    {
                        DeactivateFly();
                    }
                }
                else
                {

                    isGrounded = false;
                }
            }
            else
            {
                jumpTimer += Time.deltaTime;
                if (jumpTimer > jumpCooldown)
                {
                    jumpTimer = 0f;
                    canJump = true;
                }
            }

            if (!invincible)
            {
                return;
            }

            timer += Time.deltaTime;

            if (timer > data.invincibilityTime)
            {
                invincible = false;
                timer = 0f;
            }


            /*
            if (headCamera)
            {
                transform.position = new Vector3(headCamera.transform.position.x, transform.position.y, headCamera.transform.position.z);
                headCamera.transform.position = new Vector3(transform.position.x, headCamera.transform.position.y, transform.position.z);
            }
            */

        }

        void HolderUpdater(ItemData itemData, int count)
        {
            if(itemData == data.bombData)
            {
                AddBombs(count);
            }
            else if(itemData == data.keyData)
            {
                AddKeys(count);
            }

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

        public void Jump(GameManager.ButtonState buttonState)
        {
            if (buttonState == GameManager.ButtonState.Down)
            {
                if (isGrounded)
                {
                    if (canJump)
                    {
                        rb.AddForce(transform.up * data.jumpForce, ForceMode.VelocityChange);
                        canJump = false;
                        isGrounded = false;
                    }
                }
                else
                {
                    if (data.canFly)
                    {
                        if (flying)
                        {
                            DeactivateFly();
                        }
                        else
                        {
                            ActivateFly();
                        }
                    }
                }
            }

        }

        public void ActivateFly()
        {
            rb.useGravity = false;
            flying = true;
            rb.drag = 0.95f;
        }

        public void DeactivateFly()
        {
            rb.useGravity = true;
            flying = false;
            rb.drag = 0f;
        }


        public void Rotate(Vector2 direction2D)
        {


            if (Mathf.Abs(direction2D.x) > 0.3f)
            {
                transform.Rotate(transform.up, turnAngle * direction2D.x);
            }

            if (flying)
            {
                if (Mathf.Abs(direction2D.y) > 0.3f)
                {
                    rb.AddForce(transform.up * direction2D.y * acceleration * Time.deltaTime, ForceMode.Acceleration);

                    if (Mathf.Abs(rb.velocity.y) > maxSpeed + data.movementBoost)
                    {
                        rb.velocity = new Vector3(rb.velocity.x, Mathf.Sign(rb.velocity.y) * (maxSpeed + data.movementBoost), rb.velocity.z);
                    }



                }
            }

        }

        public void TakeDamage(float damage)
        {
            
            if (data.invincible)
            {
                return;
            }

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




        public static void UpdateHealth(float damage)
        {
            local.data.currentHealth = local.health;
            local.maxHealth = local.data.baseHealth + local.data.healthBoost;
            UpdateHealthEvent?.Invoke();
        }

        public static void Heal(float amount)
        {
            local.health += amount;
            if(local.health > local.maxHealth)
            {
                local.health = local.maxHealth;
            }

            UpdateHealth();
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
            data.bombs = number;
            UpdateInventoryEvent?.Invoke();

        }

        public void AddKeys(int number)
        {
            data.keys = number;
            UpdateInventoryEvent?.Invoke();
        }

        public void AddCoins(int number)
        {
            data.coins = number;
            UpdateInventoryEvent?.Invoke();
        }

        public void OnGameOver()
        {
            if (data.completedLevel)
            {
                data.currentHealth = health;
            }
            else
            {
                data.currentLevel = 0;
            }

            GameManager.leftJoystickEvent -= Move;
            GameManager.rightJoystickEvent -= Rotate;
            GameManager.RightThumbstickPressEvent -= Jump;

            PlayerTookDamageEvent -= UpdateHealth;
            Holder.HolderUpdate -= HolderUpdater;

            GameManager.GameOverEvent -= OnGameOver;

            local = null;
        }

    }
}

