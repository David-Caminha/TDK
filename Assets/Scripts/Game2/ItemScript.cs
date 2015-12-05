using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class ItemScript : MonoBehaviour
{

    public int amount = 0;
    public List<GameObject> parents = new List<GameObject>();
    public float buyPrice;
    public float sellPrice;
    public float startTime;
    public float repeatTime;
    public bool produces;

    void Start()
    {
        init();
        if (produces)
            InvokeRepeating("production", startTime, repeatTime);
    }

    abstract public void init(); // Override this function to intitialize values for each item

    abstract public void production(); // Override this function for specific items

    public void BuyItem()
    {
        if (Game2ControllerScript.gold >= buyPrice)
        {
            Game2ControllerScript.gold -= buyPrice;
            Game2ControllerScript.updateGold();
            amount++;
        }
    }

    public void SellItem()
    {
        if (amount > 0)
        {
            Game2ControllerScript.gold += sellPrice;
            Game2ControllerScript.updateGold();
            amount--;
        }
    }
}
