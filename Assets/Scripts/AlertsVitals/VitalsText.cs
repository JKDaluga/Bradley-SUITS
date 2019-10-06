using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VitalsText : MonoBehaviour {
    private const int NUM_VITALS = 16;
    public VitalsSlot[] vitalsSlots = new VitalsSlot[NUM_VITALS];
  
    public string tempText;
    public string BatteryLife;
    public string BatteryCapacity;
    public string OxygenLife;
    public string EVA;
    public string H2OLife;
    public string O2Pressure;
    public string O2Rate;
    public string H2OGas;
    public string H2OLiquid;
    public string SOPPressure;
    public string SOPSuitRate;
    public string InternalSuitPressure;
    public string FanTachometer;
    public string SUBPressure;
    public string SUBTemp;
    public string BPM;
    

    public void UpdateText()
    {
        //Debug.Log(vitalsSlots.Length + "vialsslsls");
        tempText = "" + DataController.data.data[0].t_sub;
        //EVA.text = "" + DataController.data.data[0].extraVehicularActivityTime;
        EVA = "";

        //Example Pie Meter
        string[] timeArray = DataController.data.data[0].t_battery.Split(':');
        float seconds = float.Parse(timeArray[0]) * 3600 + float.Parse(timeArray[1]) * 60 + float.Parse(timeArray[2]);
        float tempFillAmount = seconds / 36000;
        if (tempFillAmount > 1) tempFillAmount = 1;
        else if(tempFillAmount < 0) tempFillAmount = 0;
        int tempOrder = 11;
        AddToList("Battery", "Time Remaining", "" + DataController.data.data[0].t_battery, tempFillAmount, tempOrder, true);

        //Example Gauge Meter
        float batcap = DataController.data.data[0].cap_battery;
        tempFillAmount = DataController.data.data[0].cap_battery / 30f;
        if (tempFillAmount > 1) tempFillAmount = 1;
        else if (tempFillAmount < 0) tempFillAmount = 0;
        tempOrder = 12;
        AddToList("Battery", "Capacity", DataController.data.data[0].cap_battery.ToString() + " amp-hr", tempFillAmount, tempOrder, true);

        float o2p = DataController.data.data[0].p_o2;
        o2p -= 850;
        o2p /= (100/0.7f);
        tempOrder = 2;
        tempFillAmount = o2p / 2 + 0.5f;
        if (tempFillAmount > .9) tempFillAmount = .9f;
        else if (tempFillAmount < .1f) tempFillAmount = .1f;
        AddToList("Oxygen", "Current Pressure", DataController.data.data[0].p_o2.ToString() + " psia", tempFillAmount, tempOrder, false);

        float o2r = DataController.data.data[0].rate_o2;
        o2r -= 0.75f;
        o2r /= 0.25f / 0.7f;
        tempOrder = 1;
        tempFillAmount = o2r / 2 + 0.5f;
        AddToList("Oxygen", "Current Rate", DataController.data.data[0].rate_o2.ToString() + " psi/min", tempFillAmount, tempOrder, false);

        string[] timeArray3 = DataController.data.data[0].t_oxygen.Split(':');
        seconds = float.Parse(timeArray3[0]) * 3600 + float.Parse(timeArray3[1]) * 60 + float.Parse(timeArray3[2]);
        tempFillAmount = seconds /*float.Parse(DataController.data.data[0].t_water)*/ / 36000f;
        if (tempFillAmount > 1) tempFillAmount = 1;
        else if (tempFillAmount < 0) tempFillAmount = 0;
        tempOrder = 0;
        AddToList("Oxygen", "Time Remaining", DataController.data.data[0].t_oxygen, tempFillAmount, tempOrder, true);

        string[] timeArray2 = DataController.data.data[0].t_water.Split(':');
        seconds = float.Parse(timeArray2[0]) * 3600f + float.Parse(timeArray2[1]) * 60f + float.Parse(timeArray2 [2]);
        tempFillAmount = seconds / 36000f;
        if (tempFillAmount > 1) tempFillAmount = 1;
        else if (tempFillAmount < 0) tempFillAmount = 0;
        tempOrder = 3;
        AddToList("H2O", "Time Remaining", DataController.data.data[0].t_water, tempFillAmount, tempOrder, true);

        float h20pg = DataController.data.data[0].p_h2o_g;
        h20pg -= 15;
        h20pg /= 1 / 0.7f;
        tempOrder = 5;
        tempFillAmount = h20pg / 2 + 0.5f;
        AddToList("H2O", "Gas Pressure", DataController.data.data[0].p_h2o_g.ToString() + " psia", tempFillAmount, tempOrder, false);

        float h20pl = DataController.data.data[0].p_h2o_l;
        h20pl -= 15;
        h20pl /= 1 / 0.7f;
        tempOrder = 4;
        tempFillAmount = h20pl / 2 + 0.5f;
        AddToList("H2O", "Liquid Pressure", DataController.data.data[0].p_h2o_l.ToString() + " psia", tempFillAmount, tempOrder, false);

        float fanSpeed = DataController.data.data[0].v_fan;
        fanSpeed -=25000;
        fanSpeed /= 15000 / 0.7f;
        tempOrder = 10;
        tempFillAmount = fanSpeed / 10000;
        AddToList("Fan", "Current RPM", DataController.data.data[0].v_fan.ToString() + " RPM", tempFillAmount, tempOrder, false);

        float subp = DataController.data.data[0].p_sub;
        subp -= 3;
        subp /= 1 / 0.7f;
        tempOrder = 6;
        tempFillAmount = subp / 2 + 2f;
        AddToList("Sub", "SubPressure", DataController.data.data[0].p_sub.ToString() + " psia", tempFillAmount, tempOrder, false);

        float sopp = DataController.data.data[0].p_sop;
        sopp -= 850;
        sopp /= 100 / 0.7f;
        tempOrder = 9;
        tempFillAmount = sopp / 2 + 0.5f;
        AddToList("SOP", "Current Pressure", DataController.data.data[0].p_sop.ToString() + " psia", tempFillAmount, tempOrder, false);

        float sopr = DataController.data.data[0].rate_sop;
        sopr -= .75f;
        sopr /= .25f / 0.7f;
        tempOrder = 8;
        tempFillAmount = sopr / 2 + 0.5f;
        AddToList("SOP", "Rate", DataController.data.data[0].rate_sop.ToString() + "psi/min", tempFillAmount, tempOrder, false);

        float suitp = DataController.data.data[0].p_suit;
        suitp -= 3;
        suitp /= 1 / 0.7f;
        tempOrder = 7;
        tempFillAmount = suitp / 2 + 0.5f;
        AddToList("Suit", "Current Pressure", DataController.data.data[0].p_suit.ToString() + " psid", tempFillAmount, tempOrder, false);

    }

    void AddToList(string title, string subTitle, string value, float fillAmount, int i, bool isPie)
    {
        //fill the vitalslot
        vitalsSlots[i].fillamount = fillAmount;
        vitalsSlots[i].order = i;
        //vitalsSlots[i].title.text = title;
        vitalsSlots[i].subTitle.text = subTitle;
        vitalsSlots[i].value.text = value;
        vitalsSlots[i].isPie = isPie;
        if (vitalsSlots[i].isPie)
        {
            vitalsSlots[i].pie.gameObject.SetActive(true);
            vitalsSlots[i].pie.GetComponent<PieMeterController>().SetProgressPercentage(vitalsSlots[i].fillamount);
            vitalsSlots[i].gauge.gameObject.SetActive(false);
            if (vitalsSlots[i].fillamount <= .25f)
            {
                vitalsSlots[i].value.color = new Color(1.0f, 0.172549f, 0.3333333f);
            }
            else
            {
                vitalsSlots[i].value.color = new Color(0.0f, 0.9803922f, 0.3411765f);
            }
        }
        else
        {
            vitalsSlots[i].gauge.gameObject.SetActive(true);
            vitalsSlots[i].gauge.GetComponent<GaugeMeterController>().SetProgressPercentage(vitalsSlots[i].fillamount);
            vitalsSlots[i].pie.gameObject.SetActive(false);
            if (vitalsSlots[i].fillamount <= .15f || vitalsSlots[i].fillamount >= .85f)
            {
                vitalsSlots[i].value.color = new Color(1.0f, 0.172549f, 0.3333333f);
            }
            else
            {
                vitalsSlots[i].value.color = new Color(0.0f, 0.9803922f, 0.3411765f);
            }
        }

    }
}
