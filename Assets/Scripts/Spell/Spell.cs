using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class Spell : Interactable

    {
        public ParticleSystem spellVFX;
        public AudioSource spellSFX;
        public SpellData data;

        public GameObject idleSpell;

        public bool onCooldown;

        public Collider grabDetectionCollider;

        [HideInInspector] public bool isCasting;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Use()
        {
            base.Use();

            if (onCooldown)
            {
                return;
            }

            if(data.manaCost > Player.local.mana)
            {
                return;
            }
            else
            {

                switch (data.spellMode)
                {
                    case SpellData.SpellMode.Continuous:

                        if(data.manaCost != 0)
                        {
                            StartCoroutine(ManaDrain());
                        }
                        

                        break;

                    case SpellData.SpellMode.Instant:
                        Player.local.mana -= data.manaCost;
                        if (data.cooldown != 0)
                        {
                            onCooldown = true;
                            StartCoroutine(Cooldown(data.cooldown));
                        }
                        break;


                }

                if (spellSFX)
                {
                    spellSFX.Play();
                }

                if (spellVFX)
                {
                    spellVFX.Play();
                }

                isCasting = true;

                OnCast();
            }



        }

        public override void StopUsing()
        {
            base.StopUsing();

            isCasting = false;

            if(data.spellMode == SpellData.SpellMode.Continuous)
            {
                if(data.manaCost != 0)
                {
                    StopCoroutine(ManaDrain());
                }
                
                if(data.cooldown != 0)
                {
                    onCooldown = true;
                    StartCoroutine(Cooldown(data.cooldown));
                }

                if (spellSFX)
                {
                    spellSFX.Stop();
                }

                if (spellVFX)
                {
                    spellVFX.Stop();
                }


            }

            OnCastStop();
        }

        public virtual void OnCast()
        {

        }

        public virtual void OnCastStop()
        {

        }

        public IEnumerator Cooldown(float timer)
        {
            yield return new WaitForSeconds(timer);
            onCooldown = false;
        }

        public IEnumerator ManaDrain()
        {
            while(Player.local.mana > 0)
            {
                Player.local.mana -= data.manaCost * Time.deltaTime;
                yield return null;
            }
        }

        public void OnSpellGrabbed(Grabber grabber)
        {
            idleSpell.SetActive(false);
            grabDetectionCollider.enabled = false;
            holder = grabber;
            transform.position = grabber.transform.position;
            transform.rotation = grabber.transform.rotation;
            transform.parent = grabber.transform;
        }


    }
}

