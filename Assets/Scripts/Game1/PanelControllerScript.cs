using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelControllerScript : MonoBehaviour {

	public void TogglePanel (GameObject panel)
	{
		var secondaryPanels = GameObject.FindGameObjectsWithTag("SecondaryPanel");
        var tertiaryPanels = GameObject.FindGameObjectsWithTag("TertiaryPanel");
        if (panel.CompareTag("SecondaryPanel"))
        {
            for (int i = 0; i < secondaryPanels.Length; i++)
            {
                if (panel != secondaryPanels[i])
                    secondaryPanels[i].SetActive(false);
            }
            for(int j = 0; j < tertiaryPanels.Length; j++)
            {
                tertiaryPanels[j].SetActive(false);
            }
        }
        if(panel.CompareTag("TertiaryPanel"))
        {
            for (int j = 0; j < tertiaryPanels.Length; j++)
            {
                if(panel != tertiaryPanels[j])
                    tertiaryPanels[j].SetActive(false);
            }
        }
		panel.SetActive (!panel.activeSelf);
	}
}
