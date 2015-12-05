using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game1ControllerScript : MonoBehaviour {

	public static float netWorth = 50000f;
    public static GameObject[] workerGroups;
    public static int numBuildings = 1;
	static float profit;
    static float overallHappiness = 0.75f;

	public static Text text;

    void Awake()
    {
        workerGroups = GameObject.FindGameObjectsWithTag("WorkerGroup");
        text = GameObject.FindGameObjectWithTag("Money").GetComponent<Text>();
    }

	void Start()
	{
		CalculateProfit ();
        InvokeRepeating("EndMonth", 30f, 30f);
        UpdateText();
    }

	void EndMonth ()
	{
        CalculateProfit();
        Debug.Log(profit);
		netWorth += profit;
        UpdateText();
	}

    public static void UpdateText()
    {
        text.text = "Cash " + netWorth + "$";
    }

	public static void CalculateProfit()
	{
        profit = 0;
        for(int i = 0; i < workerGroups.Length; i++)
        {
            Debug.Log(workerGroups[i].transform.childCount);
            var script = workerGroups[i].GetComponent<WorkerScript>();
            float gainPerWorker = ((script.gain * (script.baseHappiness * script.happinessModifier)) - (script.wages + script.bonusPay));
            profit += gainPerWorker * workerGroups[i].transform.childCount;
        }
    }

    public static void CalculateHappiness(float value)
    {
        overallHappiness = (overallHappiness * numBuildings + value) / (numBuildings + 1);
        numBuildings++;
        for (int i = 0; i < workerGroups.Length; i++)
        {
            var script = workerGroups[i].GetComponent<WorkerScript>();
            script.happinessModifier = 1 + overallHappiness - 0.5f;
            script.happiness.text = "Happiness " + (script.baseHappiness * script.happinessModifier * 100) + "%";
        }
    }
}
