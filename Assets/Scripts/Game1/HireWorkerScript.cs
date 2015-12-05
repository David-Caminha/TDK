using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HireWorkerScript : MonoBehaviour {

	public GameObject workerPrefab;
	public List<Transform> workSpaces = new List<Transform>();

	public void AddWorker (Transform workerGroup)
	{
		if (workSpaces.Count > 0 && Game1ControllerScript.netWorth > 500)
		{
			Game1ControllerScript.netWorth -= 500;
            Game1ControllerScript.UpdateText();
			var worker = (GameObject) Instantiate (workerPrefab, workSpaces [0].position, Quaternion.identity);
            worker.transform.parent = workerGroup;
			Destroy (workSpaces [0].gameObject);
			workSpaces.RemoveAt (0);
		}
	}
}