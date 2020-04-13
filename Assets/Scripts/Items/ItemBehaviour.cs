using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    [HideInInspector] public Item itemController;
    protected bool initialized;


    // Start is called before the first frame update
    public virtual void Start()
    {
        itemController = gameObject.GetComponent<Item>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!initialized)
        {
            Initialize();
        }
    }

    public virtual void Initialize()
    {

        initialized = true;
    }
}
