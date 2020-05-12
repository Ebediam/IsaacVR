using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class Damager : MonoBehaviour
    {
        public EffectsData effectsData;


        private void OnCollisionEnter(Collision collision)
        {
            CollisionEnterEvent(collision);
        }

        public virtual void CollisionEnterEvent(Collision collision)
        {
            Debug.Log("DAMAGER FIRED");

            if (effectsData)
            {
                Debug.Log("EFFECTSDATA FOUND");
                if (effectsData.materialEffects.Count > 0)
                {
                    Debug.Log("effectsdata list not empty");
                    foreach (MaterialEffectData materialEffectData in effectsData.materialEffects)
                    {


                        Debug.Log("Considering material " + materialEffectData.material.name + " and material " + collision.collider.sharedMaterial);
                        if (materialEffectData.material == collision.collider.sharedMaterial)
                        {
                            Debug.Log("Material checks");
                            GameObject vfx = Instantiate(materialEffectData.VFX);
                            vfx.transform.position = collision.GetContact(0).point;

                            GameObject sfx = Instantiate(materialEffectData.SFX);
                            sfx.transform.position = collision.GetContact(0).point;
                            break;

                        }
                    }

                }
            }
            else
            {
                Debug.Log("EFFECTSDATA NOT FOUND");
            }


        }


    }
}

