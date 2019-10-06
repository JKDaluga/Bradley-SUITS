using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Note to Future Jason. This page is garbage please delete
/// </summary>
public class VoiceCommands : MonoBehaviour {
    public Image procedureScreen1;
    public Image procedureScreen2;
    public Text proc1Name;
    public Text proc2Name;
    public Text proc1Steps;
    public Text proc2Steps;
    public Image onboardingScreen;
    public AnimationManager onboardScreen;
    public GameObject stepsScreen;
	public AnimationManager slideInInstructions;
    public StepIterator stepIterator;
	public AnimationManager FadeInProcedures;

	void Start()
	{
		slideInInstructions = GameObject.FindWithTag("Manager").GetComponent<AnimationManager>();
		FadeInProcedures = GameObject.FindWithTag("Manager").GetComponent<AnimationManager>();
	}

    void begin()
    {
        onboardScreen.PlayAnimationFadeOut(onboardingScreen);
        proc1Name.text = DataController.pdata.procedures[1].name;
        proc1Steps.text = "" + DataController.pdata.procedures[1].steps + " Steps";
        proc2Name.text = DataController.pdata.procedures[2].name;
        proc2Steps.text = "" + DataController.pdata.procedures[2].steps + " Steps";
		FadeInProcedures.PlayAnimationFadeIn(procedureScreen1);
        FadeInProcedures.PlayAnimationFadeIn(procedureScreen2);
    }

    void startProcedure()
    {
        stepIterator.reset();
	    slideInInstructions.SlideInInstructions ();
    }

    void stopProcedure()
    {
        stepsScreen.SetActive(false);
		//slideInInstructions.SlideOutInstructions ();
    }


}
