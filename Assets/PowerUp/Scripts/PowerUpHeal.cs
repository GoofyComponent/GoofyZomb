using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUpHeal : MonoBehaviour{
    
    public GameObject player;

    [Header(("Parametres du powerup"))]
    [SerializeField] bool useStaticValue = false;
    [SerializeField] int staticValue = 35;
    [SerializeField] int minHealth = 10;
    [SerializeField] int maxHealth = 90;
    private int healthModifier;

    private void OnCollisionEnter(Collision collision)  
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            if (useStaticValue){
                healthModifier = staticValue;
            }
            else {
                healthModifier = Random.Range(minHealth, maxHealth);
            }
            collision.gameObject.GetComponent<PlayerDatas>().gainHealth(healthModifier);
        }
    }

   
}