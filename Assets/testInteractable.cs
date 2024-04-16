using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInteractable : interactable
{

    public HUDShop HUDShop;

    void Start() {
        HUDShop = GameObject.FindObjectOfType(typeof(HUDShop)) as HUDShop;
    }

    public override void OnFocus()
    {
        print("LOOKING AT " + gameObject.name);
        HUDShop.displayTheText();
    }

    public override void OnInteract()
    {
        print("INTERACTED WITH " + gameObject.name);
    }

    public override void OnLoseFocus()
    {
        print("STOPPED LOOKING AT " + gameObject.name);
        HUDShop.removeTheText();
    }
}
