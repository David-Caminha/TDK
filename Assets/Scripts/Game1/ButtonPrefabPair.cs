using UnityEngine;
using System.Collections;

public class ButtonPrefabPair {

	public GameObject button;
	public GameObject prefab;
	
	public ButtonPrefabPair(GameObject button, GameObject prefab)
	{
		this.button = button;
		this.prefab = prefab;
	}
}
