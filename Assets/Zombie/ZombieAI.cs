using UnityEngine;
using UnityEngine.AI;
using System;
using System.Threading;
using Data = PlayerDatas;

public class ZombieAI : MonoBehaviour
{
    [Header(("Reload per attack"))]
    [SerializeField] private float Tempo = 2.0f;
    [Header(("Infos du Zombie"))]
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int health = 100;

    public NavMeshAgent navMeshAgent;
    public int damage = 1;
    public float attackRadius = 2f;
    private GameObject player;
    public Animator anim;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > attackRadius)
        {
            navMeshAgent.SetDestination(player.transform.position);
            anim.SetBool("isWalking", true);
        }
    }
    void wait(){
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
    }

    public bool loseHealth(int damage)
    {
        health = health - damage;
        if(health>0){
            return false;
        }else if (health==0){
            return true;
        }
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), player.GetComponent<Collider>(),true);
            anim.SetBool("isWalking", false);
            Data.loseHealth(10);
            print(Data.health);
            Invoke("wait",Tempo);


        }
        
    }
}



