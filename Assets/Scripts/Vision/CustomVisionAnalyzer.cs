using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class CustomVisionAnalyzer : MonoBehaviour
{
    /// <summary>
    /// Unique instance of this class
    /// </summary>
    public static CustomVisionAnalyzer instance;

    /// <summary>
    /// The predecetion key for the Azure server
    /// </summary>
    private string predictionKey = "3e0f466c3e9342b28a6469f3ed9abdd0";

    /// <summary>
    /// The URL for the Azure server
    /// </summary>
    private string predectionEndpoint = "https://customvisionsuits.cognitiveservices.azure.com/customvision/v3.0/Prediction/9fcd12e5-203d-4f22-b1ac-b203f05cfac8/detect/iterations/ChairTest/url";

    /// <summary>
    /// Byte array of the image to submit for analysis
    /// </summary>
    [HideInInspector] public byte[] imageBytes;

    private void Awake()
    {
        // Ses the class up as a singleton
        instance = this;
    }

    /// <summary>
    /// Call the servie to submit the image
    /// </summary>
    /// <param name="imagePath">The file path to the image</param>
    /// <returns></returns>
    public IEnumerator AnalyzeLasImageCaptured(string imagePath)
    {
        WWWForm webForm = new WWWForm();

        using (UnityWebRequest unityWebRequest =
            UnityWebRequest.Post(predectionEndpoint, webForm))
        {
            // Gets a byte array out of the saved image
            imageBytes = GetImageAsByteArray(imagePath);

            unityWebRequest.SetRequestHeader("Content-Type",
                "application/json");
            unityWebRequest.SetRequestHeader("Predection-Key", predictionKey);

            // The upload handler will help uploading the byte array with the request
            unityWebRequest.uploadHandler = new UploadHandlerRaw(imageBytes);
            unityWebRequest.uploadHandler.contentType = "application/json";

            // The download handler will help receiving the analysis from Azure
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            // Send the request
            yield return unityWebRequest.SendWebRequest();
            
            string jsonResponse = unityWebRequest.downloadHandler.text;
            
            // Creaete a texture. Texture size does not mattre since LoadImage will replace with the incoming image size
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(imageBytes);
            SceneOrganiser.Instance.quadRenderer.material.SetTexture("_MainTex", tex);
            
            // The response will be in JSON forma,therefor it needs to be deserialized
            AnalysisRootObject analysisRootObject = new AnalysisRootObject();
            analysisRootObject = JsonConvert.DeserializeObject<AnalysisRootObject>(jsonResponse);
            
            //Sceneorganizer.Instance.FinaliseLabel(analysisRootObject);
        }
    }

    static byte[] GetImageAsByteArray(string imageFilePath)
    {
        FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
        
        BinaryReader binaryReader = new BinaryReader(fileStream);

        return binaryReader.ReadBytes((int) fileStream.Length);
    }
}
