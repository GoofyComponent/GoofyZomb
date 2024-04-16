using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private GameObject player;
    private GameObject weapon;
    // Start is called before the first frame update
    void Start()
    {
        //find weapon by the Tag
        weapon = GameObject.FindGameObjectWithTag("Weapon");
        // Find player by the Tag
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        // on vérifie si les raycast de la Camera soit sur l'objet
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        print("Money: " + player.GetComponent<PlayerDatas>().money);
        if (Physics.Raycast(ray, out hit))
        {
            // Vérifiez si l'objet touché est l'objet actuel (l'objet sur lequel le script est attaché) par le raycast
            if (hit.collider.gameObject == gameObject)
            {
                // si on enclenche le bouton E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // on vérifie si le joueur a assez d'argent
                    if (player.GetComponent<PlayerDatas>().money >= 10)
                    {
                        // on retire 10$ au joueur
                        player.GetComponent<PlayerDatas>().loseMoney(10);
                        // on ajoute 30 balles à l'arme
                        weapon.GetComponent<Tir>().AddAmmo(30);
                    }
                }
            }
        }
        

    }
}
