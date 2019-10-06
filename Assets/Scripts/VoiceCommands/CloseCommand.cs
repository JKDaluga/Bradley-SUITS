using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseCommand : MonoBehaviour {
    public GameObject vitalsPanel;
	public AnimationManager slideOutVitals;

	void Start()
	{
		slideOutVitals = GameObject.FindWithTag("Manager").GetComponent<AnimationManager>();
	}

    void CloseVitals()
    {
		AudioLibrary.CloseMenuSFX ();

    }
}
