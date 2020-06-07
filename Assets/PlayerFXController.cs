using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using BOIVR;
using UnityEngine;

public class PlayerFXController : MonoBehaviour
{
    public Player player;
    public AudioSource damagedSFX;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GameOverEvent += OnGameOver;
        player.PlayerTookDamageEvent += OnDamageReceived;
    }

    void OnGameOver()
    {
        GameManager.GameOverEvent -= OnGameOver;
        player.PlayerTookDamageEvent -= OnDamageReceived;
    }

    void OnDamageReceived(float damage)
    {
        if(damage > 0)
        {
            damagedSFX.Play();
        }


    }

}
