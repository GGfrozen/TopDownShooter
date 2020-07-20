using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    [SerializeField] Slider health;
    [SerializeField] Text bulletsCount;

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        health.maxValue = player.ReturnHealth();
        player.OnPlayerHealth += ChangePlayerSlider;
        bulletsCount.text = " " + player.ReturnBulletCount();
        player.OnBulletsCountChange += ChangeBulletsCount;
    }
    private void ChangePlayerSlider()
    {        
        health.value = player.ReturnHealth();

    }
    private void ChangeBulletsCount()
    {
        bulletsCount.text = " " + player.ReturnBulletCount();
    }
}
