using System.Collections.Generic;
using UnityEngine;

public class IndividualVitalView : MonoBehaviour
{
    //How long in seconds the blocks should show before dissapearing
    private const float PIN_TIME = 10f;
    
    //The currently-showing vitals block
    //A block is one UI container for a vital. This includes the heading, and all
    //subpanels with that show the data
    public GameObject CurrentBlock = null;
    
    //Used for holding pinned blocks
    public List<GameObject> PinnedBlocks;
    
    //Countdown timer for pinning the vital block
    public float PinTime = PIN_TIME;
    public GameObject helpText;


    /// <summary>
    /// Runs before anything else in the scene
    /// </summary>
    public void Awake()
    {
        helpText.SetActive(false);
    }
    
    /// <summary>
    /// Runs once per frame
    /// </summary>
    public void Update()
    {
        //Runs the timer down
        tickCoundown();
    }

    /// <summary>
    /// Spawns and configures ann individual vitals block
    /// </summary>
    /// <param name="type">The type of block to spawn</param>
    public void SpawnBlock(string type)
    {
        Debug.Log("IndividualVitalView.SpawnBlock: Type: " + type);
        //Sets the initial configuration values
        bool isPie = false;
        float seconds = 0;
        float tempFillAmount = 0;
        string[] timeArray;
        
        //Amount of sub-panels in current block
        int subPanelAmount = 0;
        //Resets the pin timer
        PinTime = PIN_TIME; 
        
        //Plays the sound effect for viewing a vital
        AudioLibrary.OpenMenuSFX();
        
        //Removes the current block, if any
        Destroy(CurrentBlock);
        
        //Sets the help text for the individual vital block to be shown
        helpText.SetActive(true);
        
        //Spawns the panel of the given type
        CurrentBlock = SpawnPanel(type);

        //Sets the data of the block to be accurate
        switch (type)
        {
            
            case "Battery":
                //Splits the time, given as 00:00:00, into a single number
                timeArray = DataController.data.data[0].t_battery.Split(':');

                //Gets the fill amount for the graph based on the time
                tempFillAmount = CalculateTempFillFromTime(timeArray);

                //Sets the properties of the subpanel
                SetPie(true, type, 1);
                SetFillAmount(tempFillAmount, type, 1);
                SetSubtitle("Time Remaining", type, 1);
    
                //Sets the actual data on the subpanel
                CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock1").GetComponent<VitalsSlot>().value.text = DataController.data.data[0].t_battery;

                //Calculates the fill amount for the graph not relating to time
                tempFillAmount = DataController.data.data[0].cap_battery / 30f;
                if (tempFillAmount > 1) tempFillAmount = 1;
                else if (tempFillAmount < 0) tempFillAmount = 0;

                SetPie(true, type, 2);
                SetFillAmount(tempFillAmount, type, 2);
                SetSubtitle("Time Remaining", type, 2);
                CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock2").GetComponent<VitalsSlot>().value.text = DataController.data.data[0].cap_battery.ToString();

                break;

            case "H2O":
                timeArray = DataController.data.data[0].t_water.Split(':');
                
                tempFillAmount = CalculateTempFillFromTime(timeArray);

                SetPie(true, type, 1);
                SetFillAmount(tempFillAmount, type, 1);
                SetSubtitle("Time Remaining", type, 1);
                CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock1").GetComponent<VitalsSlot>().value.text = DataController.data.data[0].t_water;

                float h20pg = DataController.data.data[0].p_h2o_g;
                h20pg -= 15;
                h20pg /= 1 / 0.7f;
                tempFillAmount = h20pg / 2 + 0.5f;

                SetPie(false, type, 2);
                SetFillAmount(tempFillAmount, type, 2);
                SetSubtitle("Time Remaining", type, 2);
                CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock2").GetComponent<VitalsSlot>().value.text = DataController.data.data[0].p_h2o_g.ToString();

                float h20pl = DataController.data.data[0].p_h2o_l;
                h20pl -= 15;
                h20pl /= 1 / 0.7f;
                tempFillAmount = h20pl / 2 + 0.5f;

                SetPie(false, type, 3);
                SetFillAmount(tempFillAmount, type, 3);
                SetSubtitle("Time Remaining", type, 3);
                CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock3").GetComponent<VitalsSlot>().value.text = DataController.data.data[0].p_h2o_l.ToString();
                
                break;

            case "Oxygen":
                float o2p = DataController.data.data[0].p_o2;
                
                o2p -= 850;
                o2p /= (100 / 0.7f);
                tempFillAmount = o2p / 2 + 0.5f;
                if (tempFillAmount > .9) tempFillAmount = .9f;
                else if (tempFillAmount < .1f) tempFillAmount = .1f;
                
                SetPie(false, type, 1);
                SetFillAmount(tempFillAmount, type, 1);
                SetSubtitle("Time Remaining", type, 1);
                CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock1").GetComponent<VitalsSlot>().value.text = DataController.data.data[0].p_o2.ToString();

                float o2r = DataController.data.data[0].rate_o2;
                o2r -= 0.75f;
                o2r /= 0.25f / 0.7f;
                tempFillAmount = o2r / 2 + 0.5f;
                
                SetPie(false, type, 2);
                SetFillAmount(tempFillAmount, type, 2);
                SetSubtitle("Time Remaining", type, 2);
                CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock2").GetComponent<VitalsSlot>().value.text = DataController.data.data[0].rate_o2.ToString();

                timeArray = DataController.data.data[0].t_oxygen.Split(':');

                tempFillAmount = CalculateTempFillFromTime(timeArray);
                SetPie(true, type, 3);
                SetFillAmount(tempFillAmount, type, 3);
                SetSubtitle("Time Remaining", type, 3);
                CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock3").GetComponent<VitalsSlot>().value.text = DataController.data.data[0].t_oxygen;

                break;

            case "Suit":
                float suitp = DataController.data.data[0].p_suit;
                suitp -= 3;
                suitp /= 1 / 0.7f;
                tempFillAmount = suitp / 2 + 0.5f;

                SetPie(false, type, 1);
                SetFillAmount(tempFillAmount, type, 1);
                SetSubtitle("Time Remaining", type, 1);
                CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock1").GetComponent<VitalsSlot>().value.text = DataController.data.data[0].p_suit.ToString();

                break;

            case "SOP":
                float sopp = DataController.data.data[0].p_sop;
                sopp -= 850;
                sopp /= 100 / 0.7f;
                tempFillAmount = sopp / 2 + 0.5f;

                SetPie(false, type, 1);
                SetFillAmount(tempFillAmount, type, 1);
                SetSubtitle("Time Remaining", type, 1);
                CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock1").GetComponent<VitalsSlot>().value.text = DataController.data.data[0].p_sop.ToString();


                float sopr = DataController.data.data[0].rate_sop;
                sopr -= .75f;
                sopr /= .25f / 0.7f;
                tempFillAmount = sopr / 2 + 0.5f;

                SetPie(false, type, 2);
                SetFillAmount(tempFillAmount, type, 1);
                SetSubtitle("Time Remaining", type, 2);
                CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock2").GetComponent<VitalsSlot>().value.text = DataController.data.data[0].rate_sop.ToString();

                break;

            case "Sub":
                float subp = DataController.data.data[0].p_sub;
                subp -= 3;
                subp /= 1 / 0.7f;
                tempFillAmount = subp / 2 + 2f;

                SetPie(false, type, 1);
                SetFillAmount(tempFillAmount, type, 1);
                SetSubtitle("Time Remaining", type, 1);
                CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock1").GetComponent<VitalsSlot>().value.text = DataController.data.data[0].p_sub.ToString();

                break;

            case "Fan":
                float fanSpeed = DataController.data.data[0].v_fan;
                fanSpeed -= 25000;
                fanSpeed /= 15000 / 0.7f;
                tempFillAmount = fanSpeed / 10000;

                SetPie(false, type, 1);
                SetFillAmount(tempFillAmount, type, 1);
                SetSubtitle("Time Remaining", type, 1);
                CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock1").GetComponent<VitalsSlot>().value.text = DataController.data.data[0].v_fan.ToString();

                break;

        }
    }

    /// <summary>
    /// Takes a time in seconds and converts to to a 0-1 value for filling a graph
    /// </summary>
    /// <param name="seconds">What amount of seconds to convert</param>
    /// <returns>The amount to fill the graph</returns>
    private float CalculateTempFillFromTime(string[] timeArray)
    {
        float seconds = float.Parse(timeArray[0]) * 3600 + float.Parse(timeArray[1]) * 60 + float.Parse(timeArray[2]);
        float tempFillAmount = seconds / 36000;
        if (tempFillAmount > 1) tempFillAmount = 1;
        else if (tempFillAmount < 0) tempFillAmount = 0;

        return tempFillAmount;
    }

    /// <summary>
    /// Sets if any given vitals subpanel should display a pie chart or meter gauge
    /// </summary>
    /// <param name="isPie">Should the subpanel display a pie graph</param>
    /// <param name="type">What type of panel is being spawned</param>
    /// <param name="subpanel">Which subpanel is being configured</param>
    private void SetPie(bool isPie, string type, int subpanel)
    {
        CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock" + subpanel).GetComponent<VitalsSlot>().isPie = isPie;
    }

    /// <summary>
    /// Sets the fill amount for the graphs in the subpanels
    /// </summary>
    /// <param name="fillAmount">How much of the graph should be filled</param>
    /// <param name="type">What type of panel is being spawned</param>
    /// <param name="subpanel">Which subpanel is being configured</param>
    private void SetFillAmount(float fillAmount, string type, int subpanel)
    {
        CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock" + subpanel).GetComponent<VitalsSlot>().fillamount = fillAmount;
    }

    /// <summary>
    /// Sets the subtitle for the subpanels
    /// </summary>
    /// <param name="subtitle">What the subtitle on the subpanel to be</param>
    /// <param name="type">What type of panel is being spawned</param>
    /// <param name="subpanel">Which subpanel is being configured</param>
    private void SetSubtitle(string subtitle, string type, int subpanel)
    {
        CurrentBlock.transform.Find(type.ToLower() + "_VitalsDataBlock" + subpanel).GetComponent<VitalsSlot>().subTitle.text = subtitle;
    }
    
    /// <summary>
    /// Spawns an individual panel of a given type
    /// </summary>
    /// <param name="type">The type of panel to spawn</param>
    private GameObject SpawnPanel(string type)
    {
        GameObject toInstantiate = null;
        GameObject toReturn = null;

        //Loads the prefab from file
        toInstantiate = Resources.Load<GameObject>("Prefabs/Vitals/" + type);
        
        //Instantiates the block
        toReturn = GameObject.Instantiate(toInstantiate);
        
        //Sets the block's position to the upper right corner
        toReturn.transform.position = ((Camera.main.transform.forward * 10)
                                  + (2 * Camera.main.transform.right) + (2 * Camera.main.transform.up));
        toReturn.transform.rotation = new Quaternion(0, 0, 0, 0);
        
        //Sets the block's parent so it is always n front of the user
        toReturn.transform.parent = GameObject.Find("VitalsCanvas").transform;
        
        //Scales the panel down to not be massive
        toReturn.transform.localScale *= 0.005f;

        //Tags the block so it can be removed
        toReturn.tag = "IndividualPanel";
        
        return toReturn;
    }
    
    /// <summary>
    /// Runs the timer down
    /// </summary>
    private void tickCoundown()
    {
        //Makes sure a block has been spawned
        if (CurrentBlock != null)
        {
            PinTime = PinCountdown(PinTime);

            //If the time is 0, destory the current block
            if (PinTime <= 0)
            {
                helpText.SetActive(false);
                Destroy(CurrentBlock);
            }
        }
    }

    /// <summary>
    /// Sticks the current panel in place
    /// </summary>
    private void PinIndividualPanel()
    {
        //Makes sure there is a panel spawned to pin
        if (CurrentBlock != null)
        {
            helpText.SetActive(false);

            //Adds the panel to the pinned list
            PinnedBlocks.Add(CurrentBlock);
            CurrentBlock.transform.SetParent(GameObject.Find("Menu").transform);

            //Resets the current panel
            CurrentBlock = null;
        }
    }

    /// <summary>
    /// Modifies the timer's number
    /// </summary>
    /// <param name="timerSeconds">The timer to modify</param>
    /// <returns></returns>
    private float PinCountdown(float timerSeconds)
    {
        timerSeconds -= Time.deltaTime;

        return timerSeconds;
    }


    /// <summary>
    /// Removes the panel being looked at. Currently not working
    /// </summary>
    private void RemoveFocusedPanel()
    {
        //Sets up a RaycastHit to be used in the subsequent raycast
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 30.0f,
            Physics.DefaultRaycastLayers))
        {
            //Makes sure the object hit is an individual panel, and if so, destroys it
            if (hit.collider.gameObject.CompareTag("IndividualPanel"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    /// <summary>
    /// Removes the first panel that was pinned, reguardless of gaze direction
    /// </summary>
    private void RemoveFirstPanel()
    {
        if (PinnedBlocks.Count > 0)
        {
            GameObject[] toRemoveArray = PinnedBlocks.ToArray();
            PinnedBlocks.RemoveAt(0);
            Destroy(toRemoveArray[0]);
        }
    }
}
