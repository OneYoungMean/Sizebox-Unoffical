using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadModel 
{
    public List<SizeboxModel> SizeboxModels = new List<SizeboxModel>();

    private string WarningMessage1 = "Can't find this path.";
    private List<string> FilePaths = new List<string>();
    private List<string> BundleSavePaths;
    private string Extension;


    public LoadModel(List<string> BundleSavePaths, string Extension)//OYM：初始化
    {
        this.BundleSavePaths = BundleSavePaths;
        this.Extension = Extension;
        foreach (string m_BundleSavePath in BundleSavePaths)
        {
            if (!Directory.Exists(m_BundleSavePath))//判断文件夹是否能够访问
            {
                Directory.CreateDirectory(m_BundleSavePath);
                Debug.Log("Can't find this path.");
            }
            else
            {
                GetFileList(m_BundleSavePath);
            }

        }
        GetSizeboxModelList(FilePaths, Extension);
    }


    public List<string> GetFileList(string folder)//OYM：获取所有文件列表
    {
        string[] directories = Directory.GetDirectories(folder);//传回所有子目录的路径数组      
        foreach (string subdirectory in directories)
            FilePaths.AddRange( GetFileList(subdirectory));
        FilePaths.AddRange(Directory.GetFiles(folder));
        return FilePaths;
    }

    public List<SizeboxModel> GetSizeboxModelList(List<string> FilePaths, string Extension)//OYM：获取所有Sziebox模型
    {
        if (FilePaths.Count == 0 || Extension.Length == 0) return null;

        foreach (string m_FilePath in FilePaths) 
        {

            if (!m_FilePath.EndsWith(Extension.Substring(1))) continue;
            
            AssetBundle ABundle = AssetBundle.LoadFromFile(m_FilePath);
                
            if (ABundle == null)
            {
                Debug.Log("Error 000：Can't loading model {0}" + m_FilePath);//OYM：无法加载
            }
            else
            {
                StringHolder modelsInside = (StringHolder)ABundle.LoadAsset("modellist");
                foreach (string m_modelName in modelsInside.content)
                {
                    Texture2D Texture = ABundle.LoadAsset(m_modelName + "Thumb") as Texture2D;
                    //if(Texture == null) Texture ==standardTexture;
                    SizeboxModels.Add( new SizeboxModel(m_FilePath, m_modelName, Texture));
                }
                ABundle.Unload(false);
            }
            
        }
        return SizeboxModels;
    }


    /*
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("Files", Files);

    }
    public LoadModel(SerializationInfo info, StreamingContext context)
    {
        info.GetValue("Files", typeof(string[]));
    }
    */
}
