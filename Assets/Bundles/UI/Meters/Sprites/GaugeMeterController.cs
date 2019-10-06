using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GaugeMeterController : MonoBehaviour
{
 // Public variables
  [Header( "Needle Attributes" )]
   [Tooltip( "Width of the 'needle' that goes back and forth" )]
 	public float needleWidth = 0.05f; // [0.0f, 1.0f]

  [Header( "State Determination" )]
   //[Tooltip( "Lowest percentage that will result in a caution state/color (danger state overides this)" )]
	//public float cautionPointLow = 0.45f; // [0.0f, 1.0f] // Can be lower than 0 to have no effect
   //[Tooltip( "Highest percentage that will result in a caution state/color (danger state overides this)" )]
	//public float cautionPointHigh = 0.65f; // [0.0f, 1.0f] // Can be higher than 1 to have no effect
   [Tooltip( "Lowest percentage that will result in a danger state/color" )]
	public float dangerPointLow = 0.15f; // [0.0f, 1.0f] // Can be lower than 0 to have no effect
   [Tooltip( "Highest percentage that will result in a danger state/color" )]
	public float dangerPointHigh = 0.85f; // [0.0f, 1.0f] // Can be higher than 1 to have no effect

 // NOTE: Not tripping either the caution or danger states will result in a default stable state
  [Header( "State Colors" )]
   [Tooltip( "Color the meter will turn while in the stable state" )]
	public Color stableColor = new Color( 0.0f,0.9803922f,0.3411765f );
   [Tooltip( "Color the meter will turn while in the caution state" )]
	public Color cautionColor = new Color( 1.0f,0.8392157f,0.0f );
   [Tooltip( "Color the meter will turn while in the danger state" )]
	public Color dangerColor = new Color( 1.0f,0.172549f,0.3333333f );


 // Private variables
	public Image backFillImage; // Reference to background fill color image
	public Image progressImage; // Reference to progress meter fill color image (most important)
	public Image foreFillImage; // Reference to foreground fill color image (covers up other side of progress image)
	public Image foreLineImage; // Reference to foreground outline color image
	private float currentPercent = 1.0f; // Store current progress internalls instead of using fillamount because of the needle visualization stuff below
	private int currentState = 0; // 0 = stable, 1 = caution, 2 = danger
	private float needleWidthHalf;


 // Init by gathering references to each essential child object
	void Start()
	{
	 // NOTE: Pie meter prefabs must remain in this order with these child ojbect names (assumes each have an image component)
		backFillImage = transform.Find( "BackFill" ).GetComponent<Image>();
		progressImage = transform.Find( "Progress" ).GetComponent<Image>();
		foreFillImage = transform.Find( "ForeFill" ).GetComponent<Image>();
		foreLineImage = transform.Find( "ForeLine" ).GetComponent<Image>();
	 // Update needle effect
	  UpdateNeedleVisuals();

	 // Update state info just in case the caution/danger points are <= 1
	  UpdateProgressState();
	}


 // Checks to see if the percentage (progress image fillamount) is within any of the state ranges (stable, caution, and danger)
 // Only for internal use
	private void UpdateProgressState()
	{
		if (currentPercent <= dangerPointLow || currentPercent >= dangerPointHigh) // Dangerous levels
		{
			progressImage.color = dangerColor;
			foreLineImage.color = dangerColor;
			currentState = 2;
		}
	/* else if (currentPercent <= cautionPointLow || currentPercent >= cautionPointHigh) // Cautious levels
		{
			progressImage.color = cautionColor;
			foreLineImage.color = cautionColor;
			currentState = 1;
		}*/
	 else // Everythings all good
		{
			progressImage.color = stableColor;
			foreLineImage.color = stableColor;
			currentState = 0;
		}
	}

 // Updates overlapping images in such a way that a "needle" is seen tracking the percentage of the meter
	private void UpdateNeedleVisuals()
	{
		progressImage.fillAmount = Mathf.Min( currentPercent + needleWidth*0.5f,1.0f );
		foreFillImage.fillAmount = Mathf.Max( currentPercent - needleWidth*0.5f,0.0f );
	}

 // Returns the current state (determines color) of the meter based on the current percentage (progress image fillamount)
 // Accessible by outside scripts or via sendMessage calls
 	public float GetProgressState()
 	{
 		return currentState;
 	}

 // Returns the current percentage the meter is displaying
 // Accessible by outside scripts or via sendMessage calls
	public float GetProgressPercentage()
	{
		return currentPercent;
	}

 // Sets the new percentage for the meter and checks for new state changes
 // Accessible by outside scripts or via sendMessage calls
	public void SetProgressPercentage( float p )
	{
	 // Set the new percentage value as the fillamount of the progress image while keeping it between 0 and 1
		currentPercent = Mathf.Clamp( p,0.14f,.9f );

	// Update needle effect
		UpdateNeedleVisuals();

	 // Check to see what the new state of the meter is and adjust the color accordingly
	  UpdateProgressState();
	}

 // Add some amount to the current percentage value and update the meter
 // Accessible by outside scripts or via sendMessage calls
	public void AddProgressPercentage( float d )
	{
		SetProgressPercentage( currentPercent + d );
	}

 // Subtract some amount to the current percentage value and update the meter (inverse of add method)
 // Accessible by outside scripts or via sendMessage calls
	public void SubProgressPercentage( float d )
	{
		AddProgressPercentage( -d );
	}
}
