using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDatas : MonoBehaviour
{
    [Header(("Infos du player"))]
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public static int health = 100;
    [SerializeField] public int money = 100;
    [SerializeField] public int maxMoney = 100;



    void Start(){
        health = 100;
    }

    public static void loseHealth(int damage){
        if((health - damage)>0){
             health = health - damage;
        }
        else{
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void gainHealth(int gain){
        if((health + gain)<maxHealth){
            health = health + gain;
        }
        else{
            health = maxHealth;
        }
    }

    public void loseMoney(int value){
        if((money - value)>0){
            money = money - value;
        }
        else{
            money = 0;
        }
    }

    public void gainMoney(int value){
        money = money + value;
    }

    public void setShieldPower(int powerUpShield, float resetTime)
    {
        health += powerUpShield;
        Invoke("resetShield", resetTime);
    }

    public void resetShield(){
        print("reset");
        health = maxHealth;
    }
}
