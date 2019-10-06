using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour {

    public Text text;
    private int index;
    public string[] steps;

	// Use this for initialization
	void Start () {
        index = 0;
        //text.text = steps[index];
	}

    public void next()
    {

        if (index + 1 < steps.Length)
        {
            text.text = steps[++index];
		    AudioLibrary.CompletionStepSFX ();
        }
  
    }

    public void prev()
    {
        if (index > 0) text.text = steps[--index];
    }
}
