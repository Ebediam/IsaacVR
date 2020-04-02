using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HandUIManager : MonoBehaviour
{

    public TextMeshPro healthText;
    public TextMeshPro bombText;
    // Start is called before the first frame update
    void Start()
    {
        Player.UpdateHealthEvent += UpdateHealthUIText;
        Player.UpdateInventoryEvent += UpdateBombText;
    }

    public void UpdateHealthUIText()
    {
        string _healthText = Player.local.health.ToString();

        healthText.text = _healthText;
    }

    public void UpdateBombText()
    {
        string _bombText = Player.local.data.bombs.ToString();
        bombText.text = _bombText;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
