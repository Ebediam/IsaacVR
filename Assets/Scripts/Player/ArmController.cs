using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{
    public class ArmController : MonoBehaviour
    {
        public Transform armAttachment;
        public Transform forearm;
        public Transform forearmJoint;
        public Transform armJoint;
        public GameObject joint;

        float armLength;
        float forearmLength;

        // Start is called before the first frame update
        void Start()
        {
            armLength = Vector3.Distance(transform.position, armJoint.position);
            forearmLength = Vector3.Distance(forearm.transform.position, forearmJoint.position);
        }

        // Update is called once per frame
        void Update()
        {

            transform.position = armAttachment.position;

            Vector3 jointPosition = (forearmJoint.transform.position + armJoint.transform.position) / 2f;

            float _distance = Vector3.Distance(jointPosition, transform.position);

            if (_distance < armLength)
            {
                jointPosition += (jointPosition - transform.position).normalized * ((armLength - _distance) / _distance);
            }

            _distance = Vector3.Distance(jointPosition, forearm.position);

            if (_distance < forearmLength)
            {
                jointPosition += (jointPosition - forearm.position).normalized * ((forearmLength - _distance) / _distance);
            }


            joint.transform.position = jointPosition;



            transform.LookAt(joint.transform.position, Vector3.up);
            forearm.LookAt(joint.transform.position, Vector3.up);
        }
    }
}

