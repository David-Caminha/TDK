using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BuildSpacesScript : MonoBehaviour {

	public List<ButtonPrefabPair> buildingPlaces = new List<ButtonPrefabPair>();

	public HireWorkerScript hireWorkerScript;

	public GameObject topBuilding;
	public GameObject leftBuilding;
	public GameObject rightBuilding;

    public GameObject cheapOfficePrefab;
    public GameObject greenAreaPrefab;

    public Transform mapCanvas;

	void Awake()
	{
        //Initialising the buildingPlaces list with buttons from scene
		var button = GameObject.FindGameObjectWithTag ("BuildSpaceUp");
		var pair = new ButtonPrefabPair(button, topBuilding);
		buildingPlaces.Add(pair);
		button.SetActive (false);
		button = GameObject.FindGameObjectWithTag ("BuildSpaceLeft");
		pair = new ButtonPrefabPair(button, leftBuilding);
		buildingPlaces.Add(pair);
		button.SetActive (false);
		button = GameObject.FindGameObjectWithTag ("BuildSpaceRight");
		pair = new ButtonPrefabPair(button, rightBuilding);
		buildingPlaces.Add(pair);
		button.SetActive (false);

        //Initialising the different types of builings
        var cheapOfficeInfo = ScriptableObject.CreateInstance<NewBuildingInfo>();
        cheapOfficeInfo.buildingPrefab = cheapOfficePrefab;
        cheapOfficeInfo.happinessValue = 0.20f;
        var greenAreaInfo = ScriptableObject.CreateInstance<NewBuildingInfo>();
        greenAreaInfo.buildingPrefab = greenAreaPrefab;
        greenAreaInfo.happinessValue = 0.70f;

        //Creating listeners in buttons for each building type
        var cheapOfficeButton = GameObject.Find("Cheap Office").GetComponent<Button>();
        cheapOfficeButton.onClick.AddListener(() => ShowPlaces(cheapOfficeInfo));
        var greenAreaButton = GameObject.Find("Green Area").GetComponent<Button>();
        greenAreaButton.onClick.AddListener(() => ShowPlaces(greenAreaInfo));

        //Deactivate parent of the buttons
        cheapOfficeButton.transform.parent.gameObject.SetActive(false);
    }

	public void ShowPlaces(NewBuildingInfo info)
	{
		for(int i = buildingPlaces.Count - 1; i >= 0 ; i--)
		{

			if(buildingPlaces[i].button == null)
			{
				buildingPlaces.RemoveAt(i);
			}
			else if(buildingPlaces[i].button.activeSelf)
			{
				buildingPlaces[i].button.SetActive(false);
			}
			else
			{
                var newBuilding = ScriptableObject.CreateInstance<NewBuildingInfo>();
                newBuilding.buildingPrefab = info.buildingPrefab;
                newBuilding.happinessValue = info.happinessValue;
                newBuilding.buttonPosition = (RectTransform)buildingPlaces[i].button.transform;
                newBuilding.nextBuildingPositions = buildingPlaces[i].prefab;
				var button = buildingPlaces[i].button.GetComponent<Button>();
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(() => BuyOffice(newBuilding));
				buildingPlaces[i].button.SetActive(true);
			}
		}
	}
	
	public void BuyOffice(NewBuildingInfo info)
	{
		if(Game1ControllerScript.netWorth >= 10000)
		{
            Game1ControllerScript.CalculateHappiness(info.happinessValue);
			Game1ControllerScript.netWorth -= 10000;
            Game1ControllerScript.UpdateText();
            GameObject newBuilding = (GameObject)Instantiate(info.buildingPrefab, info.buttonPosition.position, Quaternion.identity);
            GameObject nextBuildingPositions = (GameObject)Instantiate(info.nextBuildingPositions, info.buttonPosition.position, Quaternion.identity);
            foreach(Transform child in nextBuildingPositions.transform)
            {
                child.parent.SetParent(newBuilding.transform);
            }
            newBuilding.transform.parent = mapCanvas;
            Destroy(info.buttonPosition.gameObject);
            for (int i = 0; i < buildingPlaces.Count; i++)
			{
				buildingPlaces[i].button.SetActive(false);
			}
			Transform[] childPositions = newBuilding.GetComponentsInChildren<Transform> ();
			for(int i = 0; i < childPositions.Length; i++)
			{
				if(childPositions[i].CompareTag("WorkerPlace"))
				{
					hireWorkerScript.workSpaces.Add(childPositions[i]);
				}
				else if(childPositions[i].CompareTag("BuildSpaceUp"))
				{
					var pair = new ButtonPrefabPair(childPositions[i].gameObject, topBuilding);
					buildingPlaces.Add(pair);
					childPositions[i].gameObject.SetActive(false);
				}
				else if(childPositions[i].CompareTag("BuildSpaceLeft"))
				{
					var pair = new ButtonPrefabPair(childPositions[i].gameObject, leftBuilding);
					buildingPlaces.Add(pair);
					childPositions[i].gameObject.SetActive(false);
				}
				else if(childPositions[i].CompareTag("BuildSpaceRight"))
				{
					var pair = new ButtonPrefabPair(childPositions[i].gameObject, rightBuilding);
					buildingPlaces.Add(pair);
					childPositions[i].gameObject.SetActive(false);
				}
			}
		}
	}
}
