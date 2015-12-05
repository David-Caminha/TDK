using UnityEngine;
using UnityEngine.UI;

public class Game2ControllerScript : MonoBehaviour {

    public static float gold = 110f;
    public static Text goldText;


    void Awake()
    {
        goldText = GameObject.FindGameObjectWithTag("Money").GetComponent<Text>();
    }

    void Start()
    {
        updateGold();
    }

    public static void updateGold()
    {
        goldText.text = gold + " gold";
    }
}
