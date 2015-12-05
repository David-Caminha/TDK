using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChickenScript : ItemScript {

    public override void init()
    {
        buyPrice = 10f;
        sellPrice = 7f;
        startTime = 30f;
        repeatTime = 30f;
        produces = true;
    }

    public override void production()
    {
        if(parents != null && parents[0] != null && parents[1] != null)
        {
            float ratio;
            RoosterScript rScript = parents[0].GetComponent<RoosterScript>();
            if (rScript.amount > 0)
                ratio = amount / rScript.amount; // 8 hens per rooster (over means too much hens, less means too much roosters)
            else
                ratio = 0;
            if(ratio < 4)
            {
                int numChildren = Random.Range(amount / 64, amount / 4);
                for (int i = 0; i < numChildren; i++)
                {
                    float rand = Random.Range(0f, 100f);
                    if (rand < 50)
                        amount++;
                    else
                        rScript.amount++;
                }
            }
            else if (ratio < 7.5f)
            {
                int numChildren = Random.Range(amount / 2, amount);
                for (int i = 0; i < numChildren; i++)
                {
                    float rand = Random.Range(0f, 100f);
                    if (rand < 50)
                        amount++;
                    else
                        rScript.amount++;
                }
            }
            else if (7.5f <= ratio && ratio <= 8.5)
            {
                for(int i = 0; i < amount; i++)
                {
                    float rand = Random.Range(0f, 100f);
                    if (rand < 50)
                        amount++;
                    else
                        rScript.amount++;
                }
            }
            else if (ratio > 8.5f)
            {
                int numChildren = Mathf.RoundToInt(Random.Range(rScript.amount * 8, rScript.amount * 8 * 1.25f));
                for (int i = 0; i < numChildren; i++)
                {
                    float rand = Random.Range(0f, 100f);
                    if (rand < 50)
                        amount++;
                    else
                        rScript.amount++;
                }
            }
        }
    }
}
