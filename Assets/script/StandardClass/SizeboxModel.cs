using System;
using UnityEngine;

public class SizeboxModel:MonoBehaviour
{

    public string ModelPath;
    public string ModelName;
    public Texture2D ModelTexture;
    public AssetBundle ABundle;

    public SizeboxModel(string ModelPath, string ModelName, Texture2D ModelTexture)
	{
        this.ModelPath = ModelPath;
        this.ModelName = ModelName;
        this.ModelTexture = ModelTexture;
    }
}
public enum SizeboxExtension
{
    _gts, _micro, _object
}


