using UnityEngine;

public class EggScript : ItemScript {

    public override void init()
    {
        buyPrice = 3f;
        sellPrice = 1f;
        startTime = 5f;
        repeatTime = 5f;
        produces = true;
    }

    public override void production()
    {
        if(parents != null && parents[0] != null)
        {
            ChickenScript script = parents[0].GetComponent<ChickenScript>();
            float production;
            if (script.amount < 5)
                production = Random.Range(0.95f, 1.2f);
            else if(script.amount < 15)
                production = Random.Range(0.80f, 1.1f);
            else
                production = Random.Range(0.70f, 1.05f);
            int eggsProduced = Mathf.FloorToInt(script.amount * production);
            amount += eggsProduced;
        }
    }
}
