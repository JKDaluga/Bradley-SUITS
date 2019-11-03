using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// The objects in this script represent the deserialized version of the objects used bby this application

/// <summary>
/// Web request object for image ddata
/// </summary>
class MultipartObject : IMultipartFormSection
{
    public string sectionName { get; set;  }

    public byte[] sectionData { get; set;  }

    public string fileName { get; set; }

    public string contentType { get; set;  }
}

/// <summary>
/// JSON of all the Tags existing within the project
/// </summary>
public class Tags_RootObject
{
    public List<TagOfProject> Tags { get; set; }
    public int TotalTaggedImaged { get; set; }
    public int TotalUntaggedImaged { get; set; }
}

public class TagOfProject
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ImageCout { get; set; }
}

/// <summary>
/// JSON of Tag to associate to an image
/// Contains a list of hosting the tags, since multiple tags can be associated with one image
/// </summary>
public class Tag_RootObject
{
    
}
public class CustomVisionObject : MonoBehaviour
{
    
}
