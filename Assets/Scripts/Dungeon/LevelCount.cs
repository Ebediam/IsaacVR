using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCount : MonoBehaviour
{
    public TextMeshPro levelText;
    public PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        levelText.text = playerData.currentLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
