using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Enemy enemyController;

    // Start is called before the first frame update
    public virtual void Start()
    {
        enemyController = gameObject.GetComponent<Enemy>();
    }

    // Update is called once per frame
    public virtual void Update()
    {

    }
}
