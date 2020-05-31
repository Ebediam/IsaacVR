using System.Collections;
using System.Collections.Generic;
using BOIVR;
using UnityEngine;

public class SpellShooter : Spell
{
    SpellShooterData spellShooterData;
    public Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        spellShooterData = data as SpellShooterData;
    }


    public override void OnCast()
    {
        base.OnCast();

        Bullet bullet = Instantiate(spellShooterData.bulletData.prefab).GetComponent<Bullet>();

        bullet.transform.position = spawnPoint.transform.position;
        bullet.transform.rotation = spawnPoint.transform.rotation;

        bullet.rb.AddForce(bullet.transform.forward * spellShooterData.bulletSpeed, ForceMode.VelocityChange);

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
