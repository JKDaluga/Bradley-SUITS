using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class SpatialMapping : MonoBehaviour
{

    /// <summary>
    /// Allows this class to behave like a singleton
    /// </summary>
    public static SpatialMapping Instance;

    /// <summary>
    /// Used by the GazeCursor as a property with the Raycast call
    /// </summary>
    internal static int PhysicsRaycastMask;

    /// <summary>
    /// The layer to use for the spatial mapping collisions
    /// </summary>
    internal int physicsLayer = 31;

    /// <summary>
    /// Created enviornment colliders to work with physics
    /// </summary>
    private SpatialMappingCollider spatialMappingCollider;

    /// <summary>
    /// Initialize the class
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Init and configure the collider
        spatialMappingCollider =
            gameObject.GetComponent<SpatialMappingCollider>();
        spatialMappingCollider.surfaceParent = gameObject;
        spatialMappingCollider.freezeUpdates = false;
        spatialMappingCollider.layer = physicsLayer;
        
        // Define the mask
        PhysicsRaycastMask = 1 << physicsLayer;
        
        // Set the object to be active
        gameObject.SetActive(true);
    }

}
