using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueCommand : MonoBehaviour {
    public AnimationManager onboardScreen;
    public Image onboardingScreen;
    public StepIterator stepIterator;

    private void Start()
    {
        onboardScreen = GameObject.FindWithTag("Manager").GetComponent<AnimationManager>();
    }

    public void Continue()
    {
        print("gotcha boi");
        onboardScreen.PlayAnimationFadeOut(onboardingScreen);
        //stepIterator.loadStep(0);
        //stepIterator.gameObject.SetActive(true);
    }
}
