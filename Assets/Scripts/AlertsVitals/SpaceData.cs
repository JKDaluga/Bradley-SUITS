using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpaceData
{
    public float p_suit;
    public string t_battery;
    public string t_oxygen;
    public string t_water;
    public float p_sub;
    public float t_sub;
    public float v_fan;
    public float p_o2;
    public float rate_o2;
    public float cap_battery;
    public float p_h2o_g;
    public float p_h2o_l;
    public float p_sop;
    public float rate_sop;
    public int heart_bpm;
}

public class StepData
{
    public string instruction;
    public string tip;
    public string image;
}

[System.Serializable]
public class SwitchData
{
    public bool sop_on;
    public bool sspe;
    public bool fan_error;
    public bool vent_error;
    public bool vehicle_power;
    public bool h2o_off;
    public bool o2_off;
}

[System.Serializable]
public class HelpData
{
    public string name;
    public string path;
}