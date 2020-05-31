using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace BOIVR
{
    public class Enemy : Damageable
    {
        public EnemyData data;
        [HideInInspector] public EnemyManager enemyManager;
        [HideInInspector] public bool active = false;

        public Transform target;

        public Animator animator;
        public TextMeshPro enemyText;

        [HideInInspector] public bool ignoreMaxSpeed;
        [HideInInspector] public float maxSpeed;
        public List<EnemyBehaviour> enemyBehaviours = new List<EnemyBehaviour>();
        public List<EnemyAction> enemyActions = new List<EnemyAction>();


        [HideInInspector] public float actionTimer = 0;

        void Start()
        {
            

            target = Utils.GetTarget();
            currentHealth = data.hitPoints;
            rb.mass = data.mass;
            maxSpeed = data.maxSpeed;

            actionTimer = UnityEngine.Random.Range(0, data.actionCooldown);

            if (enemyText)
            {
                enemyText.text = data.name;
            }

            foreach(EnemyBehaviour behaviour in gameObject.GetComponents<EnemyBehaviour>())
            {
                enemyBehaviours.Add(behaviour);
            }

            foreach(EnemyAction action in gameObject.GetComponents<EnemyAction>())
            {
                enemyActions.Add(action);
            }
        }


        private void Update()
        {
            if (!active)
            {
                return;
            }


            if(enemyBehaviours.Count > 0)
            {
                foreach(EnemyBehaviour behaviour in enemyBehaviours)
                {
                    if (behaviour.initialized)
                    {
                        behaviour.Action();
                    }

                    
                }
            }

            if(enemyActions.Count > 0)
            {
                actionTimer += Time.deltaTime;

                if (actionTimer > data.actionCooldown)
                {
                    int random = UnityEngine.Random.Range(0, enemyActions.Count);

                    if (enemyActions[random].initialized)
                    {
                        enemyActions[random].Action();                                               
                    }
                    actionTimer = UnityEngine.Random.Range(-data.actionCooldownModifier, data.actionCooldownModifier);
                }

            }

        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.isTrigger)
            {
                return;
            }

            Player player = collision.gameObject.GetComponentInParent<Player>();

            if (!player)
            {
                return;
            }

            DamagePlayer(player);

        }

        public void DamagePlayer(Player player)
        {
            player.TakeDamage(data.damage);

            if (data.pushback)
            {
                ignoreMaxSpeed = true;
                StartCoroutine(IgnoreMaxSpeedTimer(0.5f));
                Pushback();
            }
        }

        public void ActivateEnemy()
        {
            foreach(EnemyBehaviour behaviour in enemyBehaviours)
            {
                behaviour.Initialize();
            }

            foreach(EnemyAction action in enemyActions)
            {
                action.Initialize();
            }

            active = true;



            if (animator)
            {
                animator.SetBool("isActive", true);
            }
            
        }

        public void ActivateEnemy(EnemyManager _enemyManager)
        {            
            enemyManager = _enemyManager;
            DamageableDestroyedEvent += _enemyManager.DeadEnemyListener;

            ActivateEnemy();
        }

        public void Pushback()
        {
            rb.AddForce(transform.forward * -data.pushbackVelocity, ForceMode.VelocityChange);
        }

        public IEnumerator IgnoreMaxSpeedTimer(float timer)
        {
            yield return new WaitForSeconds(timer);
            ignoreMaxSpeed = false;
        }

        public override void DestroyDamageable()
        {
            base.DestroyDamageable();
        }
    }
}

