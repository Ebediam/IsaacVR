using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BOIVR
{
    [CreateAssetMenu]
    public class MaterialEffectData : ScriptableObject
    {
        public PhysicMaterial material;
        public GameObject SFX;
        public GameObject VFX;

    }
}

