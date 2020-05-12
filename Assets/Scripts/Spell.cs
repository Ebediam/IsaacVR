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

            spellVFX.Play();
            spellSFX.Play();

            if (data.spellMode == SpellData.SpellMode.Continuous)
            {
                isCasting = true;
            }

        }

        public override void StopUsing()
        {
            base.StopUsing();

            spellVFX.Stop();
            spellSFX.Stop();

            if (data.spellMode == SpellData.SpellMode.Continuous)
            {
                isCasting = false;
            }

        }

    }
}

