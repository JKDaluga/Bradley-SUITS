using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalCommand : MonoBehaviour {

    public GameObject CriticalAlertPanel;
    public bool isactive;
    public GameObject ErrorLog;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CritAlert()
    {
        isactive = !isactive;
        CriticalAlertPanel.SetActive(isactive);
		AudioLibrary.AlertSound ();
    }

    public void ShowErrors()
    {
        ErrorLog.SetActive(true);
    }

    public void HideErrors()
    {
        ErrorLog.SetActive(false);
    }
}
