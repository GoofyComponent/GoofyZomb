using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : MonoBehaviour{
    
    public GameObject player;

    [Header(("Parametres du powerup"))]
    [SerializeField] float speedModifier = 30.0f;
    [SerializeField] float boostDuration = 3.0f;

    private void OnCollisionEnter(Collision collision)  
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<PlayerController>().setSpeedPower(speedModifier, boostDuration);
        }
    }

   
}