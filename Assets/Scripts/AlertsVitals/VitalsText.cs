using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VitalsText : MonoBehaviour
{
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


    public float GaugeDataFormatter(float dataGauge, float median, float offset, float div1, float div2, bool capped)
    {
        float temp = dataGauge;
        temp -= median;
        temp /= (offset / 0.7f);
        float tfa = temp / div1 + div2;
        if (capped && tfa > 1f)
            tfa = 1f;
        if (capped && tfa < 0f)
            tfa = 0f;
        return tfa;
    }

    public float GaugeDataFormatter(float dataGauge, float offset, bool capped)
    {
        float tfa = dataGauge;
        tfa /= offset;
        if (capped && tfa > 1f)
            tfa = 1f;
        if (capped && tfa < 0f)
            tfa = 0f;
        return tfa;
    }

    public float PieDataFormatter(string[] dataPie)
    {
        float seconds = float.Parse(dataPie[0]) * 3600 + float.Parse(dataPie[1]) * 60 + float.Parse(dataPie[2]);
        float tfa = seconds / 36000;
        if (tfa > 1) tfa = 1;
        else if (tfa < 0) tfa = 0;
        return tfa;
    }

    public void UpdateText()
    {
        tempText = "" + DataController.data.data[0].t_sub;
        EVA = "";
        float tempFillAmount;

        // PIES

        int tempOrder = 0;
        tempFillAmount = PieDataFormatter(DataController.data.data[0].t_oxygen.Split(':'));
        AddToList("Oxygen", "Time Remaining", DataController.data.data[0].t_oxygen, tempFillAmount, tempOrder, true);

        tempOrder = 3;
        tempFillAmount = PieDataFormatter(DataController.data.data[0].t_water.Split(':'));
        AddToList("H2O", "Time Remaining", DataController.data.data[0].t_water, tempFillAmount, tempOrder, true);

        tempOrder = 11;
        tempFillAmount = PieDataFormatter(DataController.data.data[0].t_battery.Split(':'));
        AddToList("Battery", "Time Remaining", "" + DataController.data.data[0].t_battery, tempFillAmount, tempOrder, true);

        // GAUGES

        tempOrder = 1;
        tempFillAmount = GaugeDataFormatter(DataController.data.data[0].rate_o2, .75f, .25f, 2, 0.5f, false);
        AddToList("Oxygen", "Current Rate", DataController.data.data[0].rate_o2.ToString() + " psi/min", tempFillAmount, tempOrder, false);

        tempOrder = 2;
        tempFillAmount = GaugeDataFormatter(DataController.data.data[0].p_o2, 850, 100, 2, .05f, true);
        AddToList("Oxygen", "Current Pressure", DataController.data.data[0].p_o2.ToString() + " psia", tempFillAmount, tempOrder, false);

        tempOrder = 4;
        tempFillAmount = GaugeDataFormatter(DataController.data.data[0].p_h2o_l, 15, 1, 2, 0.5f, false);
        AddToList("H2O", "Liquid Pressure", DataController.data.data[0].p_h2o_l.ToString() + " psia", tempFillAmount, tempOrder, false);

        tempOrder = 5;
        tempFillAmount = GaugeDataFormatter(DataController.data.data[0].p_h2o_g, 15, 1, 2, 0.5f, false);
        AddToList("H2O", "Gas Pressure", DataController.data.data[0].p_h2o_g.ToString() + " psia", tempFillAmount, tempOrder, false);

        tempOrder = 6;
        tempFillAmount = GaugeDataFormatter(DataController.data.data[0].p_sub, 3, 1, 2, 2, false);
        AddToList("Sub", "SubPressure", DataController.data.data[0].p_sub.ToString() + " psia", tempFillAmount, tempOrder, false);

        tempOrder = 7;
        tempFillAmount = GaugeDataFormatter(DataController.data.data[0].p_suit, 3, 1, 2, 0.5f, false);
        AddToList("Suit", "Current Pressure", DataController.data.data[0].p_suit.ToString() + " psid", tempFillAmount, tempOrder, false);

        tempOrder = 8;
        tempFillAmount = GaugeDataFormatter(DataController.data.data[0].rate_sop, .75f, .25f, 2, 0.5f, false);
        AddToList("SOP", "Rate", DataController.data.data[0].rate_sop.ToString() + "psi/min", tempFillAmount, tempOrder, false);

        tempOrder = 9;
        tempFillAmount = GaugeDataFormatter(DataController.data.data[0].p_sop, 850, 100, 2, 0.5f, false);
        AddToList("SOP", "Current Pressure", DataController.data.data[0].p_sop.ToString() + " psia", tempFillAmount, tempOrder, false);

        tempOrder = 10;
        tempFillAmount = GaugeDataFormatter(DataController.data.data[0].v_fan, 25000, 15000, 10000, 0, false);
        AddToList("Fan", "Current RPM", DataController.data.data[0].v_fan.ToString() + " RPM", tempFillAmount, tempOrder, false);

        tempOrder = 12;
        tempFillAmount = GaugeDataFormatter(DataController.data.data[0].cap_battery, 30f, true);
        AddToList("Battery", "Capacity", DataController.data.data[0].cap_battery.ToString() + " amp-hr", tempFillAmount, tempOrder, true);
    }

    void AddToList(string title, string subTitle, string value, float fillAmount, int i, bool isPie)
    {
        //fill the vitalslot
        vitalsSlots[i].fillamount = fillAmount;
        vitalsSlots[i].order = i;
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