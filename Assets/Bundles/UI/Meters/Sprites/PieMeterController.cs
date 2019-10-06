using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PieMeterController : MonoBehaviour
{
 // Public variables
 	[Header( "State Determination" )]
	// [Tooltip( "Lowest percentage that will result in a caution state/color (danger state overides this)" )]
	//public float cautionPoint = 0.64f; // [0.0f, 1.0f] // Can be lower than 0 to have no effect
	 [Tooltip( "Lowest percentage that will result in a danger state/color" )]
	public float dangerPoint = 0.25f; // [0.0f, 1.0f] // Can be lower than 0 to have no effect

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
	public Image foreLineImage; // Reference to foreground outline color image
	private int currentState = 0; // 0 = stable, 1 = caution, 2 = danger


 // Init by gathering references to each essential child object
	void Start()
	{
	 // NOTE: Pie meter prefabs must remain in this order with these child ojbect names (assumes each have an image component)
		backFillImage = transform.Find( "BackFill" ).GetComponent<Image>();
		progressImage = transform.Find( "Progress" ).GetComponent<Image>();
		foreLineImage = transform.Find( "ForeLine" ).GetComponent<Image>();

 	 // Defaults progress image variables to insure they are correct initially
	 // NOTE: All other variables should generally stay the same as the prefab especially image type and fill method
		progressImage.fillAmount = 1.0f;

	 // Update state info just in case the caution/danger points are <= 1
	  UpdateProgressState();
	}


 // Checks to see if the percentage (progress image fillamount) is within any of the state ranges (stable, caution, and danger)
 // Only for internal use
	private void UpdateProgressState()
	{
		if (progressImage.fillAmount <= dangerPoint) // Dangerous levels
		{
			progressImage.color = dangerColor;
			foreLineImage.color = dangerColor;
			currentState = 2;
		}
	 /*else if (progressImage.fillAmount <= cautionPoint) // Cautious levels
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
		return progressImage.fillAmount;
	}

 // Sets the new percentage for the meter and checks for new state changes
 // Accessible by outside scripts or via sendMessage calls
	public void SetProgressPercentage( float p )
	{
	 // Set the new percentage value as the fillamount of the progress image while keeping it between 0 and 1
		progressImage.fillAmount = Mathf.Clamp( p,0.0f,1.0f );

	 // Check to see what the new state of the meter is and adjust the color accordingly
	  UpdateProgressState();
	}

 // Add some amount to the current percentage value and update the meter
 // Accessible by outside scripts or via sendMessage calls
	public void AddProgressPercentage( float d )
	{
		SetProgressPercentage( progressImage.fillAmount + d );
	}

 // Subtract some amount to the current percentage value and update the meter (inverse of add method)
 // Accessible by outside scripts or via sendMessage calls
	public void SubProgressPercentage( float d )
	{
		AddProgressPercentage( -d );
	}
}
