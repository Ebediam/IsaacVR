using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOIVR
{

    public class Item : Interactable
    {
        public delegate void ItemDelegate();
        public ItemDelegate OnItemPickup;
        public ItemDelegate OnItemDrop;

        public GameObject model;
        GameObject highlight;
        public bool grababble = true;

        public ConfigurableJoint joint;

        public Rigidbody rb;

        public Transform holdPoint;


        public ItemData data;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void AddJoint(Grabber grabber)
        {
            if (joint)
            {
                RemoveJoint();
            }

            joint = gameObject.AddComponent<ConfigurableJoint>();

            joint.autoConfigureConnectedAnchor = false;
            
            joint.xDrive = CreateJointDrive(grabber);
            joint.yDrive = CreateJointDrive(grabber);
            joint.zDrive = CreateJointDrive(grabber);
            joint.rotationDriveMode = RotationDriveMode.Slerp;
            joint.slerpDrive = CreateSlerpDrive(grabber);
            

            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;

            joint.enableCollision = false;
            

            joint.connectedBody = grabber.hand.rb;

            joint.connectedMassScale = 0f;

            joint.connectedAnchor = -grabber.heldItem.holdPoint.localPosition;
            
        }


        public void RemoveJoint()
        {
            if (joint)
            {
                Destroy(joint);
            }
        }

        public JointDrive CreateJointDrive(Grabber grabber)
        {
            JointDrive drive = new JointDrive();
            drive.positionSpring = grabber.hand.player.data.grabSpring;
            drive.positionDamper = grabber.hand.player.data.grabDamper;
            drive.maximumForce = Mathf.Infinity;
            return drive;
        }

        public JointDrive CreateSlerpDrive(Grabber grabber)
        {
            JointDrive drive = new JointDrive();
            drive.positionSpring = grabber.hand.player.data.rotSpring;
            drive.positionDamper = grabber.hand.player.data.rotDamper;
            drive.maximumForce = Mathf.Infinity;
            return drive;
        }

        public void AddHighlight()
        {
            if (highlight)
            {
                highlight.SetActive(true);
            }
            else
            {

                highlight = Instantiate(model);
                highlight.transform.position = model.transform.position;
                highlight.transform.rotation = model.transform.rotation;
                highlight.transform.parent = transform;

                foreach (MeshRenderer mesh in highlight.GetComponentsInChildren<MeshRenderer>())
                {
                    mesh.material = Player.local.data.highlightMaterial;

                    mesh.transform.localScale += Vector3.one * Player.local.data.highlightThickness;

                }
            }
        }

        public void RemoveHighlight()
        {
            highlight.SetActive(false);
        }

        public void DespawnItem()
        {
            DespawnItem(0.01f);
        }

        public static Item SpawnItem(ItemData itemData)
        {
            Item _item = null;

            GameObject itemGO = Instantiate(itemData.prefab);

            _item = itemGO.GetComponent<Item>();

            return _item;
        }

        public void DespawnItem(float timer)
        {
            if (holder)
            {
                holder.Release();
            }

            Grabber.RemoveFromItemsInRange(this);

            //Destroy(gameObject, timer);
            gameObject.SetActive(false);
        }
    }

}