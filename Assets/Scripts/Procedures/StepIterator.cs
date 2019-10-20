using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StepIterator : MonoBehaviour {

    public Image image;
    public int count = 0;
    public Text InstructWithImage;
    public Text InstructWithoutImage;
    public Text tip;
    public GameObject tpanel;
    public Text stepcounter;
	public AnimationManager stepInstructionsAnimations;
    public GameObject nextStep;
    public GameObject prevStep;
    public GameObject SayExitText;
    public HoloToolkit.Unity.TextToSpeech textToSpeech;

    private void Awake()
    {
        stepInstructionsAnimations = GameObject.FindWithTag("Manager").GetComponent<AnimationManager>();
        textToSpeech = GameObject.FindWithTag("AudioManager").GetComponent<HoloToolkit.Unity.TextToSpeech>();
    }

    public void loadStep(int i)
    {
        string path = DataController.stepData.stepData[i].image;
        image.sprite = Resources.Load<Sprite>(path);
        if(path == "")
        {
            InstructWithoutImage.text = DataController.stepData.stepData[i].instruction;
            InstructWithoutImage.gameObject.SetActive(true);
            image.gameObject.SetActive(false);
            InstructWithImage.gameObject.SetActive(false);
        }
        else
        {
            InstructWithImage.text = DataController.stepData.stepData[i].instruction;
            InstructWithoutImage.gameObject.SetActive(false);
            image.gameObject.SetActive(true);
            InstructWithImage.gameObject.SetActive(true);
        }
        
        tip.text = DataController.stepData.stepData[i].tip;
        stepcounter.text = i+1 + "/" + DataController.data.data.Length;
        var msg = string.Format(DataController.stepData.stepData[i].instruction + " " + DataController.stepData.stepData[i].tip, textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);

        if (count + 1 == DataController.data.data.Length)
        {
            nextStep.SetActive(false);
            SayExitText.SetActive(true);
        }
        else
        {
            nextStep.SetActive(true);
            SayExitText.SetActive(false);
        }

        if (count == 0) prevStep.SetActive(false);
        else prevStep.SetActive(true);

        if (DataController.stepData.stepData[i].tip.Equals(""))
            tpanel.SetActive(false);
        else
            tpanel.SetActive(true);
    }

    public void next()
    {
        if (count + 1 < DataController.data.data.Length)
        {
			//stepInstructionsAnimations.SlideOutInstructions ();
            loadStep(++count);
			if (count + 1 == DataController.data.data.Length) {
				AudioLibrary.Tada ();
			} else {
				AudioLibrary.CompletionStepSFX();
			}
            //stepInstructionsAnimations.SlideInInstructions();
        }
    }

    public void prev()
    {
        if (count > 0)
        {
            loadStep(--count);
            AudioLibrary.CompletionStepSFX();
        }
    }

    public void reset()
    {
        count = 0;
        loadStep(0);
        AudioLibrary.CompletionStepSFX();
    }

    public void repeat()
    {
        var msg = string.Format(DataController.stepData.stepData[count].instruction + " " + DataController.stepData.stepData[count].tip, textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);
    }
}
