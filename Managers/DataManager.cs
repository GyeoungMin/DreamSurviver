using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonManager<DataManager>
{
    private Dictionary<string, object[]> _DataDict = new Dictionary<string, object[]>();

    public void LoadData<T>()
    {
        string dataName = typeof(T).Name;
        if (_DataDict.ContainsKey(dataName))
        {
            Debug.LogWarning($"{dataName}이(가) 이미 존재합니다.");
            return;
        }
        LoadDataSync<T>(dataName);
    }
    public void LoadData<T>(bool isAsync)
    {
        string dataName = typeof(T).Name;
        if (_DataDict.ContainsKey(dataName))
        {
            Debug.LogWarning($"{dataName}이(가) 이미 존재합니다.");
            return;
        }

        if (isAsync)
        {
            StartCoroutine(LoadDataAsync(dataName));
        }
        else
        {
            LoadDataSync<T>(dataName);
        }
    }

    private void LoadDataSync<T>(string dataName)
    {
        string fileName = DataNameToFileName(dataName);
        TextAsset json = Resources.Load<TextAsset>(fileName);
        T[] data = JsonConvert.DeserializeObject<T[]>(json.text);
        _DataDict.Add(dataName, data as object[]);
    }

    private IEnumerator LoadDataAsync(string dataName)
    {
        string fileName = DataNameToFileName(dataName);
        ResourceRequest request = Resources.LoadAsync<TextAsset>(fileName);
        yield return request;

        TextAsset json = request.asset as TextAsset;
        if (json == null)
        {
            Debug.LogWarning($"로드된 {dataName}이(가) 올바르지 않습니다.");
            yield break;
        }

        object[] data = JsonConvert.DeserializeObject<object[]>(json.text);
        _DataDict.Add(dataName, data);
    }

    public T[] GetData<T>()
    {
        string dataName = typeof(T).Name;
        if (_DataDict.ContainsKey(dataName))
        {
            return _DataDict[dataName] as T[];
        } 
        else
        {
            Debug.LogWarning($"{dataName}을(를) 로드하세요.");
            return default;
        }
    }

    private string DataNameToFileName(string dataName)
    {
        dataName = dataName.ToLower();
        int index = dataName.LastIndexOf("data");
        string fileName = dataName.Insert(index, "_");

        return fileName;
    }
}
