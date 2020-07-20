using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Image gamePanel;

   
    Player player;
    void Start()
    {       
        player = FindObjectOfType<Player>();
        player.OnPlayerDeath += RestartLevelPanel;
    }
   
    private void RestartLevelPanel()
    {
        gamePanel.gameObject.SetActive(true);
    }
    
}
