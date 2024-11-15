using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InfoManager : SingletonManager<InfoManager>
{
    private Dictionary<string, object> infoDict = new Dictionary<string, object>();
    public List<string> InfoNames = new List<string>();

    public bool LoadInfo<T>(string infoName = null) where T : new()
    {
        if (string.IsNullOrEmpty(infoName))
        {
            infoName = typeof(T).Name;
        }
        if (infoDict.ContainsKey(infoName))
        {
            infoDict.Remove(infoName);
        }
        string fileName = InfoNameToFileName(infoName);
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");

        if (File.Exists(filePath))
        {
            Debug.Log($"<color=yellow>Load existing user data for {infoName}</color>");
            string json = File.ReadAllText(filePath);
            List<T> info = JsonConvert.DeserializeObject<List<T>>(json);
            InfoNames.Add(infoName);
            infoDict.Add(infoName, info);
            return true;
        }
        else
        {
            Debug.Log($"<color=yellow>No existing data found for {infoName}, creating new</color>");
            List<T> info = new List<T>();
            infoDict.Add(infoName, info);
            InfoNames.Add(infoName);
            SaveLocal(infoName);
            return false;
        }
    }

    public List<T> GetInfo<T>(string infoName = null)
    {
        if (string.IsNullOrEmpty(infoName))
        {
            infoName = typeof(T).Name;
        }
        if (infoDict.ContainsKey(infoName))
        {
            return infoDict[infoName] as List<T>;
        }
        else
        {
            Debug.LogWarning($"{infoName} is not Loaded.");
            return default;
        }
    }

    public void SaveInfo<T>(List<T> infos, string infoName = null)
    {
        if(string.IsNullOrEmpty(infoName))
        {
            infoName = typeof(T).Name;
        }
        Debug.Log(infoName);
        infoDict[infoName] = infos;
        SaveLocal(infoName);
    }

    private void SaveLocal(string infoName)
    {
        Debug.Log("<color=yellow>Saving data locally</color>");
        string fileName = InfoNameToFileName(infoName);
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");
        Debug.Log($"Save path: {filePath}");

        if (infoDict.TryGetValue(infoName, out object info))
        {
            // Serialize the data to JSON
            string json = JsonConvert.SerializeObject(info);
            // Write JSON data to file
            File.WriteAllText(filePath, json);
            Debug.Log("Save complete!");
        }
        else
        {
            Debug.LogWarning($"{infoName} is not Loaded.");
        }
    }

    private string InfoNameToFileName(string infoName)
    {
        string fileName = infoName.ToLower();
        if(fileName.EndsWith("info"))
        {
            int index = fileName.LastIndexOf("info");
            fileName = fileName.Insert(index, "_");
        }
        else{
            fileName = fileName + "_info";
        }
        return fileName;
    }
}
