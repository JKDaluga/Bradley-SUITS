using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GazeCursor : MonoBehaviour
{

    /// <summary>
    /// The cursor (this object) mesh renderer
    /// </summary>
    private MeshRenderer meshRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        // Grab the mesh renderer that is on he same object as this script
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        
        // Set the cursor reference
        SceneOrganiser.Instance.cursor = gameObject;
        gameObject.GetComponent<Renderer>().material.color = Color.green;
        
        // The size of the cursor
        gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        // Do a raycast into the world based onn the user's head position and orientation
        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        RaycastHit gazeHitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out gazeHitInfo, 30.0f, SpatialMapping.PhysicsRaycastMask))
        {
            // If the raycast hit a hologram, display the cursor mesh
            meshRenderer.enabled = true;
            // Move the cursor to the point where the raycast hit
            transform.position = gazeHitInfo.point;
            // Rotate the cursor to hug the surface of the hologram
            transform.rotation =
                Quaternion.FromToRotation(Vector3.up, gazeHitInfo.normal);
        }
        else
        {
            // If the raycast did not hit a hologram, hide he cursor mesh
            meshRenderer.enabled = false;
        }
    }
}
