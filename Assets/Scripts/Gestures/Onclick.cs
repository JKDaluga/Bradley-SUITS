using UnityEngine;
using UnityEngine.XR.WSA.Input;


public class Onclick : MonoBehaviour
{
    private GestureRecognizer gestureRecognizer;

    public void OnInputClicked()

    {
        
    }

    // Use this for initialization
    void Start () {
        //basically say simulate 
        //the user saying "next" 
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);

        gestureRecognizer.Tapped += (args) =>
        {
            
            this.BroadcastMessage("next");
        };

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
