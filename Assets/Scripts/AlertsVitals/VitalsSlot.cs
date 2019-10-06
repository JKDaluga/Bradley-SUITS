using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VitalsSlot : MonoBehaviour {

        public Text title;
        public Text subTitle;
        public Text value;
        public PieMeterController pie;
        public GaugeMeterController gauge;
        public float fillamount;
        public int order;
        public bool isPie;

}
