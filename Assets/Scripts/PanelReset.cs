using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelReset : MonoBehaviour
{

    public GameObject dimPanel;
    public GameObject vitalsPanel;
    public GameObject onboardingPanel;
    public GameObject criticalAlertPanel;
    public GameObject allAlertsPanel;
    public GameObject stepPanel;
    public GameObject proceduceScreen;
    public GameObject text;

	// Use this for initialization
	void Start () {
        dimPanel.SetActive(false);
        vitalsPanel.SetActive(true);
        onboardingPanel.SetActive(true);
	    criticalAlertPanel.SetActive(true);
        allAlertsPanel.SetActive(false);
        stepPanel.SetActive(false);
        //proceduceScreen.SetActive(false);
        text.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
