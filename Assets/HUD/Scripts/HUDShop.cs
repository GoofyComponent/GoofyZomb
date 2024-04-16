using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDShop : MonoBehaviour
{
    public TMP_Text textDisplay;

    public void displayTheText()
    {
        print("cacrashavantoupas");
        textDisplay.GetComponent<TMP_Text>().text = "Buy ammos (10 cred)? press E";
    }

    public void removeTheText()
    {
        textDisplay.GetComponent<TMP_Text>().text = "";
    }
}
