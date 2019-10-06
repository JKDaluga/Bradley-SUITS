using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsCommand : MonoBehaviour {

	public GameObject stepPanel;
	public bool isOpen;

	// hide the instructions panel
	void HideInstructions()
	{
		stepPanel.SetActive (false);
	}

	// show the instructions panel
	void ShowInstructions()
	{
		stepPanel.SetActive (true);
	}
}
