using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShield : MonoBehaviour{
    
    public GameObject player;

    [Header(("Parametres du powerup"))]
    [SerializeField] int shieldModifier = 500;
    [SerializeField] float boostDuration = 5.0f;

    private void OnCollisionEnter(Collision collision)  
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<PlayerDatas>().setShieldPower(shieldModifier, boostDuration);
        }
    }

   
}