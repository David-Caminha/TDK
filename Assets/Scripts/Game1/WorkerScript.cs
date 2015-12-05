using UnityEngine;
using UnityEngine.UI;

public class WorkerScript : MonoBehaviour {

    public HireWorkerScript hireWorkerScript;

    public float chanceToQuit = 5f;
    public float wages = 600f;
    public float bonusPay = 0f;
    public float gain = 700f;
    public float baseHappiness = 0.95f;
    public float happinessModifier = 1.0f;
    public Text salary;
    public Text happiness;

    void Start()
    {
        salary.text = "Salary " + (wages + bonusPay);
        happiness.text = "Happiness " + (baseHappiness * happinessModifier * 100) + "%";
        InvokeRepeating("Quit", 29f, 30f);
    }

    void Quit()
    {
        if (this.transform.childCount > 0)
        {
            float rand = Random.Range(0.0f, 100.0f);
            if (rand < chanceToQuit)
            {
                var workers = this.gameObject.GetComponentsInChildren<Transform>();
                int index = Random.Range(1, (workers.Length - 1));
                var workSpace = new GameObject("Place");
                workSpace.transform.position = workers[index].position;
                hireWorkerScript.workSpaces.Add(workSpace.transform);
                Destroy(workers[index].gameObject);
            }
        }
    }

    public void ChangeWages(int amount)
    {
        if (amount > 0)
        {
            chanceToQuit--;
            baseHappiness += 0.01f;
        }
        else
        {
            chanceToQuit++;
            baseHappiness -= 0.01f;
        }
        bonusPay += amount;
        salary.text = "Salary " + (wages + bonusPay);
        happiness.text = "Happiness " + (baseHappiness * happinessModifier * 100) + "%";
    }
}
