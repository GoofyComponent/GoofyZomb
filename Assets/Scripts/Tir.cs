using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tir : MonoBehaviour {

public GameObject BulletHolePrefab;
public GameObject BulletBloodPrefab;
public float FireRate = 0.5f;
private float nextFire = 0.0f;
public AudioClip GunShot;
private bool FullAuto = false;
private float Nextreload = 0f;
public float ReloadRate = 1f;
public int clip = 30;
public int maxclip = 30;
public AudioClip reloadsound;
public int reserve = 270;

private GameObject SpawnManager;
private GameObject Player;

    void Start(){
        SpawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(Input.GetButton("Fire1") && Time.time > nextFire && clip > 0)
        {
            nextFire = Time.time + FireRate;
            GetComponent<AudioSource>().PlayOneShot(GunShot);
            Vector2 ScreenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
            RaycastHit hit;
            Ray ray;
            ray = Camera.main.ScreenPointToRay(ScreenCenterPoint);
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject.tag == "zombie_entity")
                {
                    // hit.rigidbody.AddForceAtPosition(transform.TransformDirection(-Vector3.forward) * 1000, hit.normal);
                    bool healthRemaining = hit.transform.gameObject.GetComponent<ZombieAI>().loseHealth(25);
                    if (healthRemaining == true){
                        Destroy(hit.transform.gameObject);
                        SpawnManager.GetComponent<SpawnManager>().removeOneZombie();
                        Player.GetComponent<PlayerDatas>().gainMoney(2);
                    }
                GameObject bulletBlood = Instantiate(BulletBloodPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)) as GameObject;
                Destroy(bulletBlood, 0.1f);
                }
                else if(hit.transform.gameObject.tag == "Wall")
                {
                    GameObject bulletHole = Instantiate(BulletHolePrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)) as GameObject;
                    Destroy(bulletHole, 5f);
                }
            }
            clip -= 1;
        }
        if(Input.GetKeyDown("v"))
        {
            FullAuto = !FullAuto;
        }
        if (FullAuto == true)
        {
            FireRate = 0.10f;
        }
        else
        {
            FireRate = 0.5f;
        }
        if(Input.GetKeyDown("r") && Time.time > Nextreload && clip != maxclip && reserve > 0)
        {
            Nextreload = Time.time + ReloadRate;
            GetComponent<AudioSource>().PlayOneShot(reloadsound);
            if (reserve + clip > 30)
            {
                reserve -= maxclip - clip;
                clip += maxclip - clip;
            }
            else if(reserve + clip <= 30)
            {
                clip += reserve;
                reserve = 0;
            }
        }
    }
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 130, 25),"Munitions : " + clip + " / " + reserve);
    }
    public void AddAmmo(int ammo)
    {
        reserve += ammo;
        if (reserve > 270)
        {
            reserve = 270;
        }
    }
}