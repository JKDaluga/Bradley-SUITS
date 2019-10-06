using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinScript : MonoBehaviour
{

    public void pinProcedure()
    {        
        // Do a raycast into the world that will only hit the Spatial Mapping mesh.
        var gazeDirection = Camera.main.transform.forward;
        this.transform.position = Camera.main.transform.position + gazeDirection.normalized * 11;


        //AudioLibrary.AlertSound();
        

        // Rotate this object's parent object to face the user.
        Quaternion toQuat = Quaternion.LookRotation(Camera.main.transform.forward);

        this.transform.rotation = toQuat;
    }
}
