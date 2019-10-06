using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour {

    public GameObject instructionsPanel;
    private Animator anim;
    private Animator animInstructions;
    private Animator animNotifications;
    public GameObject newVitalsPanel;
    public GameObject notifyPanel;

    float fadeTime = 1f;
    Color colorToFadeTo;

    // Use this for initialization
    void Start()
    {
        //unpause the game on start
        Time.timeScale = 1;
        //get the animator component
        anim = newVitalsPanel.GetComponent<Animator>();
        animInstructions = instructionsPanel.GetComponent<Animator>();
        animNotifications = notifyPanel.GetComponent<Animator>();
        //disable it on start to stop it from playing the default animation
        anim.enabled = false;
        animNotifications.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //pause on escape key press(INPUTS)
        if (Input.GetKeyUp(KeyCode.Escape)) {
            SlideIn();
        }
        //unpause if the escape key is pressed
        else if (Input.GetKeyUp(KeyCode.Escape)) {
            SlideOut();
        }
    }
    //transparent
    public void PlayAnimationFadeOut(Image panel)
    {
        colorToFadeTo = new Color(1f, 1f, 1f, 0f);
        panel.CrossFadeColor(colorToFadeTo, fadeTime, true, true);
        StartCoroutine(TurnOff(panel.gameObject));
    }

    IEnumerator TurnOff(GameObject gameObject)
    {
        yield return new WaitForSeconds(fadeTime);
        gameObject.SetActive(false);

    }

    //opaqe
    public void PlayAnimationFadeIn(Image panel)
	{
		colorToFadeTo = new Color (1f, 1f, 1f, 1f);
		panel.CrossFadeColor (colorToFadeTo, fadeTime, true, true);
        Transform[] children = panel.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(true);
        }
    }

	//slide in vital info
	public void SlideIn()
	{
		//enable the animator component
		anim.enabled = true;
        //play the Slidein animation
        anim.Play("NewVitalsMenuSlideIn");
		//freeze the timescale
		Time.timeScale = 0;
	}

	//slide out vital info
		public void SlideOut()
	{
		//play the SlideOut animation
		anim.enabled = true;
		anim.Play ("NewVitalsMenuSlideOut");
		//set back the time scale to normal time scale
		Time.timeScale = 1;
	}

	public void SlideInInstructions(){
        //enable the animator component
        animInstructions.enabled = true;
		//play the Slidein animation
		animInstructions.Play ("SlideInInstructions");
		//freeze the timescale
		Time.timeScale = 0;
	}

	public void SlideOutInstructions()
	{
        //enable the animator component
        animInstructions.enabled = true;
        //play the Slidein animation
        animInstructions.Play ("SlideOutInstructions");
		//freeze the timescale
		Time.timeScale = 1;
	}

	public void SlideInNotificationsPanel()
	{
		animNotifications.enabled = true;
		animNotifications.Play("NotificationAnimationSlideIn");
		Time.timeScale = 1;
	}

	public void SlideOutNotificationsPanel()
	{
		animNotifications.enabled = true;
		animNotifications.Play("NotificationAnimationSlideOut");
		Time.timeScale = 1;
	}
	/*
	public void SlideInNewVitals(){
		//enable the animator component
		anim.enabled = true;
		//play the Slidein animation
		anim.Play ("SlideInNewVitals");
		//freeze the timescale
		Time.timeScale = 0;
	}

	public void SlideOutNewVitals()
	{
		//enable the animator component
		anim.enabled = true;
		//play the Slidein animation
		anim.Play ("SlideOutNewVitals");
		//freeze the timescale
		Time.timeScale = 1;
	}
	*/




}
