using System.Collections;
using System.Collections.Generic;
using BOIVR;
using UnityEngine;

public class Heart : Item
{
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        OnItemPickup += OnPickUp;
    }

    public void OnPickUp()
    {
        OnItemPickup -= OnPickUp;

        Player.Heal(health);

        

        DespawnItem();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
