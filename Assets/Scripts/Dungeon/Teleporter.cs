using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class Teleporter : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 10)
            {
                Player.local.data.completedLevel = true;
                GameManager.GameOver();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

