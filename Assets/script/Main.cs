using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    public List<string> PathOfGTSModels = new List<string>();
    public List<string> PathOfMaleNPCs = new List<string>();
    public List<string> PathOfFemaleNPCs = new List<string>();
    public List<string> PathOfPlayerModels = new List<string>();
    public List<string> PathOfObjects = new List<string>();

    private string folderSaves = "SavedScenes";
    private string folderScreenshots = "Screenshots";
    private string folderSounds = "Sounds";

    private List<string> SizeboxModelPath = new List<string>();//OYM：标准目录的模型
    
    private void Awake()
    {
        string version = "Sizebox Unoffical V" + SizeboxConfig.GetVersionNumber();
        Cursor.visible = true;

        SizeboxModelPath.Add(Application.dataPath+ "/../Models/");

        foreach (string m_Path in SizeboxModelPath)
        {
            PathOfGTSModels.Add(m_Path + "Giantess/");
            PathOfMaleNPCs.Add(m_Path + "MaleNPC/");
            PathOfFemaleNPCs.Add(m_Path + "FemaleNPC/");
            PathOfPlayerModels.Add(m_Path + "Player/");
            PathOfObjects.Add(m_Path + "Objects/");
        }

    }
    void Start()
    {
        LoadModel PlayerModels = new LoadModel(PathOfPlayerModels, SizeboxExtension._micro.ToString());
    }

    void Update()
    {
        
    }
}
