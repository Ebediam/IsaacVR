using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HandUIManager : MonoBehaviour
{

    public TextMeshPro healthText;
    public TextMeshPro bombText;
    public TextMeshPro keyText;
    // Start is called before the first frame update
    void Start()
    {
        Player.UpdateHealthEvent += UpdateHealthUIText;
        Player.UpdateInventoryEvent += UpdateBombText;
        Player.UpdateInventoryEvent += UpdateKeyText;
    
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

    public void UpdateKeyText()
    {
        string _keyText = Player.local.data.keys.ToString();
        keyText.text = _keyText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
