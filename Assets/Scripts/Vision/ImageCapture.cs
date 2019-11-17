using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using UnityEngine.XR.WSA.Input;

public class ImageCapture : MonoBehaviour
{

    /// <summary>
    /// Allows this class to behave like a singleton
    /// </summary>
    public static ImageCapture Instance;

    /// <summary>
    /// Keep couts of he taps for image renaing
    /// </summary>
    private int captureCount = 0;

    /// <summary>
    /// Photo Capture object
    /// </summary>
    private PhotoCapture photoCaptureObject = null;

    /// <summary>
    /// Allows gestures ecognitio in Hololens
    /// </summary>
    private GestureRecognizer recognizer;

    /// <summary>
    /// Flagging if the capture loop is running or not
    /// </summary>
    internal bool captureIsActive;

    /// <summary>
    /// File pat of current analysed photo
    /// </summary>
    internal string filePath = string.Empty;


    /// <summary>
    /// called on init
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Clean up the LocalScale folder of this application from all photos stored
        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            try
            {
                file.Delete();
            }
            catch (Exception)
            {
                Debug.LogFormat("Cannot delete file: ", file.Name);
            }
        }
        
        // Subscribing to the Microsoft Hololens API gesture recognize to track user gestures
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.Tapped += TapHandler;
        recognizer.StartCapturingGestures();
    }

    private void TapHandler(TappedEventArgs obj)
    {
        if (!captureIsActive)
        {
            captureIsActive = true;
            
            // Set the cursor color to red
            SceneOrganiser.Instance.cursor.GetComponent<Renderer>().material
                .color = Color.red;
            // Begin the capture loop
            Invoke("ExecuteImageCaptureAndAnalysis", 0);
        }
    }

    private void ExecuteImageCapureAndAnalysis()
    {
        // Create label in world space using the ResultsLabel class
        // Invisible at this point, but in the correct location
        SceneOrganiser.Instance.PlaceAnalsisLabel();

        // Set the camera resolution to b the highest possible
        Resolution cameraResolution = PhotoCapture.SupportedResolutions
            .OrderByDescending((res) => res.width * res.height).First();
        Texture2D targettTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        
        // Begin the capture process, and set the format
        PhotoCapture.CreateAsync(true, delegate(PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;

            CameraParameters camParameters = new CameraParameters()
            {
                hologramOpacity = 1.0f,
                cameraResolutionWidth = targettTexture.width,
                cameraResolutionHeight = targettTexture.height,
                pixelFormat = CapturePixelFormat.BGRA32
            };
            
            // Capture the image from the camera ad save it in the App internal folder
            captureObject.StartPhotoModeAsync(camParameters,
                delegate(PhotoCapture.PhotoCaptureResult result)
                {
                    string fileName = string.Format(@"CapturedImage{0}.jpg", captureCount);
                    filePath = Path.Combine(Application.persistentDataPath,
                        fileName);
                    ++captureCount;
                    photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
                });
        });
    }
    
    /// <summary>
    /// Register the full execution of the Photo Capture
    /// </summary>
    /// <param name="result"></param>
    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        try
        {
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }
        catch (Exception e)
        {
            Debug.Log("Exception capturing photo to disk");
        }
    }

    /// <summary>
    /// The camera photo mode has stopped after capture.
    /// Begin the image analysis
    /// </summary>
    /// <param name="result"></param>
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        Debug.Log("Stopped Photo Mode");
            
        photoCaptureObject.Dispose();
        photoCaptureObject = null;

        StartCoroutine(
            CustomVisionAnalyzer.instance
                .AnalyzeLasImageCaptured(filePath));
    }

    /// <summary>
    /// Stops all capture pending actions
    /// </summary>
    internal void ResetImageCapture()
    {
        captureIsActive = false;
        
        // Set the cursor color to green
        SceneOrganiser.Instance.cursor.GetComponent<Renderer>().material.color =
            Color.green;
        
        // Stop the capture loop if active
        CancelInvoke();
    }
}
