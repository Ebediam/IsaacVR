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

        [HideInInspector] public float maxSpeed;
        public List<EnemyBehaviour> enemyBehaviours = new List<EnemyBehaviour>();
        public List<EnemyAction> enemyActions = new List<EnemyAction>();


        [HideInInspector] public float actionTimer = 0;

        void Start()
        {

            currentHealth = data.hitPoints;
            rb.mass = data.weightClass.mass;
            rb.drag = data.weightClass.drag;
            maxSpeed = data.maxSpeed;

            actionTimer = UnityEngine.Random.Range(0, data.actionCooldown);

            if (enemyText)
            {
                enemyText.text = data.name;
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
                Pushback();
            }
        }

        public void ActivateEnemy()
        {
            if(data.targetType == EnemyData.Target.Player)
            {
                target = Utils.GetTarget();
            }
            else if(data.targetType == EnemyData.Target.NearestEnemy)
            {
                FindNearestEnemy();
            }
            
            foreach (EnemyBehaviour behaviour in gameObject.GetComponents<EnemyBehaviour>())
            {
                enemyBehaviours.Add(behaviour);
                behaviour.Initialize();
            }   


            foreach (EnemyAction action in gameObject.GetComponents<EnemyAction>())
            {
                enemyActions.Add(action);
                action.Initialize();
            }

            active = true;


            if (animator)
            {
                animator.SetBool("isActive", true);
            }
            
        }

        public void FindNearestEnemy()
        {

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

        public void Kill()
        {
            DestroyDamageable();
        }
        public override void DestroyDamageable()
        {
            base.DestroyDamageable();
        }
    }
}

