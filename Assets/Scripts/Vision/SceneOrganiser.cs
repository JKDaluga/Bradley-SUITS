using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;
using UnityEngine.XR.WSA;

public class SceneOrganiser : MonoBehaviour
{
    /// <summary>
    /// Lets the class act as a singleton
    /// </summary>
    public static SceneOrganiser Instance;

    /// <summary>
    /// The curcor atached to the main camera
    /// </summary>
    internal GameObject cursor;

    /// <summary>
    /// Label used to display the analysis of objects in the real world
    /// </summary>
    public GameObject label;
    
    /// <summary>
    /// Reference to the last label positioned
    /// </summary>
    internal Transform lastLabelPlaced;

    /// <summary>
    /// Reference to the last label positioned
    /// </summary>
    internal TextMesh LastLabelPlacedText;

    /// <summary>
    /// Current threshold accepted for displaying the label.
    /// Reduce this value to display the recognition more often
    /// </summary>
    internal float probabilityThreshold = 0.8f;
    
    /// <summary>
    /// The quad object hosting the imposed image captured
    /// </summary>
    private GameObject quad;

    /// <summary>
    /// Renderer of the quad object
    /// </summary>
    internal Renderer quadRenderer;

    /// <summary>
    /// Called on init
    /// </summary>
    private void Awake()
    {
        Instance = this;

        gameObject.AddComponent<ImageCapture>();

        gameObject.AddComponent<CustomVisionAnalyzer>();

        gameObject.AddComponent<CustomVisionObject>();
    }

    /// <summary>
    /// Instatiae a label in the appropriate location relatice to he main camera
    /// </summary>
    public void PlaceAnalsisLabel()
    {
        lastLabelPlaced = Instantiate(label.transform,
            cursor.transform.position, transform.rotation);
        LastLabelPlacedText = lastLabelPlaced.GetComponent<TextMesh>();
        LastLabelPlacedText.text = "";
        lastLabelPlaced.transform.localScale =
            new Vector3(0.005f, 0.005f, 0.005f);
        
        // Create a GameObject to which he texture can be applied
        quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        Material m = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
        quadRenderer.material = m;
        
        // Here you ca set the transparency of the quad.
        float transparency = 0f;
        quadRenderer.material.color = new Color(1, 1, 1, transparency);
        
        // Set the position and scale of the quad depending on user position
        quad.transform.parent = transform;
        quad.transform.rotation = transform.rotation;
        
        // The quad is positioned slightly forward in front of the user
        quad.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);
        
        // The quad scale has been set via experimentation to allow the image
        // on the quad to be as preisley imposed onto the real world as possible
        quad.transform.localScale = new Vector3(3f, 1.65f, 1f);
        quad.transform.parent = null;
    }

    /// <summary>
    /// Sets the Tags as Text of the last label created
    /// </summary>
    /// <param name="analysisObject"></param>
    public void FinalizeLabel(AnalysisRootObject analysisObject)
    {
        if (analysisObject.predictions != null)
        {
            LastLabelPlacedText = lastLabelPlaced.GetComponent<TextMesh>();
            // Sort the predictions to locate the highest one
            List<Prediction> sortedPredictions = new List<Prediction>();
            sortedPredictions = 
                analysisObject.predictions.OrderBy(p => p.probability).ToList();
            Prediction bestPrediction = new Prediction();
            bestPrediction = sortedPredictions[sortedPredictions.Count - 1];

            if (bestPrediction.probability > probabilityThreshold)
            {
                quadRenderer = quad.GetComponent<Renderer>() as Renderer;
                Bounds quadBounds = quadRenderer.bounds;
                
                // Position the label as close as possible to th bounding box of the quad
                // At this point it will not consider depth
                lastLabelPlaced.transform.parent = quad.transform;
                lastLabelPlaced.transform.localPosition =
                    CalculateBoundingBoxPosition(quadBounds, bestPrediction.boundingBox);
                // Set the tag text
                LastLabelPlacedText.text = bestPrediction.tagName;
                
                // Cast a ray from the user's head to the currently placed label.
                // It should hit the object detected by the Service.
                // Reposition the label to where the ray hit the object - depth
                Vector3 headPosition = Camera.main.transform.position;
                RaycastHit objHitInfo;
                Vector3 objDirection = lastLabelPlaced.position;
                if (Physics.Raycast(headPosition, objDirection, out objHitInfo, 30.0f, SpatialMapping.PhysicsRaycastMask))
                {
                    lastLabelPlaced.position = objHitInfo.point;
                }
            }
        }
        
        // Reset the color of the cursor
        cursor.GetComponent<Renderer>().material.color = Color.green;
        
        // Stop the analysis process
        ImageCapture.Instance.ResetImageCapture();
    }

    public Vector3 CalculateBoundingBoxPosition(Bounds b, BoundingBox boundingBox)
    {
        double centerFromLeft = boundingBox.left + (boundingBox.width / 2);
        double centerFromTop = boundingBox.top + (boundingBox.height / 2);
        double quadWidth = b.size.normalized.x;
        double quadHeight = b.size.normalized.y;
        double normalisedPos_X = (quadWidth * centerFromLeft) - (quadWidth / 2);
        double normalisedPos_Y = (quadHeight * centerFromTop) - (quadHeight / 2);

        return new Vector3((float)normalisedPos_X, (float)normalisedPos_Y, 0);
    }
}
