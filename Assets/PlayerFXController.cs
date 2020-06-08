using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using BOIVR;
using OVR.OpenVR;
using UnityEngine;

public class PlayerFXController : MonoBehaviour
{
    public Player player;
    public AudioSource damagedSFX;
    public AudioSource jumpSFX;
    public AudioSource pickupSFX;
    public AudioSource powerupSFX;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GameOverEvent += OnGameOver;
        player.PlayerTookDamageEvent += OnDamageReceived;
        Player.playerJumpEvent += OnJump;
        Player.UpdateInventoryEvent += OnPickup;
        Powerup.PowerupEvent += OnPowerup;

    }

    void OnJump()
    {
        jumpSFX.Play();
    }

    void OnPickup()
    {
        pickupSFX.Play();
    }

    void OnPowerup()
    {
        powerupSFX.Play();
    }
    void OnGameOver()
    {
        GameManager.GameOverEvent -= OnGameOver;
        player.PlayerTookDamageEvent -= OnDamageReceived;
        Player.playerJumpEvent -= OnJump;
        Player.UpdateInventoryEvent -= OnPickup;
        Powerup.PowerupEvent -= OnPowerup;
    }

    void OnDamageReceived(float damage)
    {
        if(damage > 0)
        {
            damagedSFX.Play();
        }


    }

}
