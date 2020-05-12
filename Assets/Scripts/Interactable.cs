using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class Interactable : MonoBehaviour
    {

        [HideInInspector] public Grabber holder;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void Use()
        {

        }

        public virtual void AltUse()
        {

        }

        public virtual void StopAltUse()
        {

        }

        public virtual void StopUsing()
        {

        }
    }
}

