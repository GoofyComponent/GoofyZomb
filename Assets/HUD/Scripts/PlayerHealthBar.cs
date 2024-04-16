using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data = PlayerDatas;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider healthBar;
    PlayerDatas playerDatas;

    void Start ()
    {
        playerDatas = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDatas>();
    }

    void Update()
    {
        healthBar.value = Data.health;
    }
}
