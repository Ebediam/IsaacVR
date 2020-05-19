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

        public bool grababble = true;

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


        public void AddHighlight()
        {
            GameObject highlight = Instantiate(model);
            highlight.transform.position = model.transform.position;
            highlight.transform.rotation = model.transform.rotation;
            highlight.transform.parent = transform;

           

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

            Destroy(gameObject, timer);
        }
    }

}