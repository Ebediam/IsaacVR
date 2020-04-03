using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Enemy enemyController;
    public Transform target;
    protected bool initialized;

    // Start is called before the first frame update
    public virtual void Start()
    {
        enemyController = gameObject.GetComponent<Enemy>();
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
        if (!target)
        {
            target = Utils.GetTarget();
        }
        initialized = true;
    }
}
