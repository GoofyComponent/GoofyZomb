using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Waves : MonoBehaviour
{
    public TMP_Text waveDisplay;
    SpawnManager spawnerManager;
    void Start()
    {
        spawnerManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        
    }

    void Update()
    {
        waveDisplay.GetComponent<TMP_Text>().text = "Wave " + spawnerManager.getWave().ToString();
    }
}
