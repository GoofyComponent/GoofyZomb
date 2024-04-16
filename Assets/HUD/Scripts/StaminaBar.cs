using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data = PlayerController;

public class StaminaBar : MonoBehaviour
{
    public Slider staminabar;
    PlayerController playerController;

    void Start ()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        staminabar.value = Data.currentStamina;
    }
}
