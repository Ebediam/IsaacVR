using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public float timeToDespawn = 2f;
    public float damage = 0.5f;

    float timer = 0f;
    bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            return;
        }

        timer += Time.deltaTime;

        if(timer >= timeToDespawn)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 2f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!active)
        {
            return;
        }

        Player player = other.GetComponentInParent<Player>();

        if (player)
        {
            player.TakeDamage(damage);
        }



    }
}
