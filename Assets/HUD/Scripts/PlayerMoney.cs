using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMoney : MonoBehaviour
{
    public TMP_Text moneyDisplay;
    PlayerDatas playerDatas;

    void Start()
    {
        playerDatas = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDatas>();
    }

    void Update()
    {
        moneyDisplay.GetComponent<TMP_Text>().text = "Money : " + playerDatas.money.ToString();
    }
}
