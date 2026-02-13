using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Firat0667.WesternRoyaleLib.Patterns;

public class SaveManager : FoundationSingleton<SaveManager>, IFoundationSingleton
{
    private string savePath;

    public bool Initialized { get ; set ; }

    private void Awake()
    {
        if (!Initialized)
        {
            savePath = Application.persistentDataPath + "/saveData.json";
            Initialized = true;
        }
    }

    /// <summary>
    /// Saves a single value.
    /// </summary>
    public void Save<T>(string key, T data)
    {
        SaveData saveData = LoadAllData();
        saveData.StoreData(key, data);
        SaveToFile(saveData);
    }

    /// <summary>
    /// Saves a tuple (2 values).
    /// </summary>
    public void Save<T1, T2>(string key, T1 data1, T2 data2)
    {
        var tupleData = new Tuple<T1, T2>(data1, data2);
        Save(key, tupleData);
    }

    /// <summary>
    /// Saves a triple (3 values).
    /// </summary>
    public void Save<T1, T2, T3>(string key, T1 data1, T2 data2, T3 data3)
    {
        var tripleData = new Triple<T1, T2, T3>(data1, data2, data3);
        Save(key, tripleData);
    }

    /// <summary>
    /// Saves a list of values.
    /// </summary>
    public void SaveList<T>(string key, List<T> list)
    {
        Save(key, list);
    }

    /// <summary>
    /// Saves a tuple of lists.
    /// </summary>
    public void SaveList<T>(string key, List<T> list1, List<T> list2)
    {
        var tupleList = new Tuple<List<T>, List<T>>(list1, list2);
        Save(key, tupleList);
    }

    /// <summary>
    /// Saves a triple of lists.
    /// </summary>
    public void SaveList<T>(string key, List<T> list1, List<T> list2, List<T> list3)
    {
        var tripleList = new Triple<List<T>, List<T>, List<T>>(list1, list2, list3);
        Save(key, tripleList);
    }

    /// <summary>
    /// Loads a single value.
    /// </summary>
    public T Load<T>(string key)
    {
        SaveData saveData = LoadAllData();
        if (!saveData.HasKey(key))
        {
            Debug.LogWarning($"[SaveManager] Key '{key}' not found!");
            return default;
        }
        return saveData.RetrieveData<T>(key);
    }

    /// <summary>
    /// Loads a list of values.
    /// </summary>
    public List<T> LoadList<T>(string key)
    {
        return Load<List<T>>(key) ?? new List<T>();
    }

    /// <summary>
    /// Loads all saved data.
    /// </summary>
    private SaveData LoadAllData()
    {
        if (!File.Exists(savePath)) return new SaveData();
        string json = File.ReadAllText(savePath);
        if (string.IsNullOrWhiteSpace(json)) return new SaveData();

        var data = JsonUtility.FromJson<SaveData>(json);
        return data ?? new SaveData();
    }


    /// <summary>
    /// Saves the data to a JSON file.
    /// </summary>
    private void SaveToFile(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("[SaveManager] Data saved successfully!");
    }

    /// <summary>
    /// Deletes all saved data.
    /// </summary>
    public void DeleteAllData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("[SaveManager] All data deleted!");
        }
    }
}

//[Serializable]
//public class SaveData
//{
//    private Dictionary<string, string> _data = new();

//    /// <summary>
//    /// Stores serialized data in dictionary.
//    /// </summary>
//    public void StoreData<T>(string key, T data)
//    {
//        string json = JsonUtility.ToJson(data);
//        _data[key] = json;
//    }

//    /// <summary>
//    /// Retrieves and deserializes data.
//    /// </summary>
//    public T RetrieveData<T>(string key)
//    {
//        if (_data.TryGetValue(key, out string json))
//        {
//            return JsonUtility.FromJson<T>(json);
//        }

//        Debug.LogWarning($"[SaveData] Key not found: {key}");
//        return default;
//    }

//    /// <summary>
//    /// Checks if a key exists in the save data.
//    /// </summary>
//    public bool HasKey(string key)
//    {
//        return _data.ContainsKey(key);
//    }
//}
[System.Serializable]
public class SaveData : ISerializationCallbackReceiver
{
    [System.Serializable]
    public struct Entry { public string key; public string value; }

    [SerializeField] private List<Entry> entries = new List<Entry>();

    private Dictionary<string, string> map = new Dictionary<string, string>();

    public void StoreData<T>(string key, T data)
    {
        string json = JsonUtility.ToJson(data);
        map[key] = json;
    }

    public T RetrieveData<T>(string key)
    {
        if (!map.TryGetValue(key, out var json)) return default;
        return JsonUtility.FromJson<T>(json);
    }

    public bool HasKey(string key) => map.ContainsKey(key);

    public void OnBeforeSerialize()
    {
        entries.Clear();
        foreach (var kv in map)
            entries.Add(new Entry { key = kv.Key, value = kv.Value });
    }

    public void OnAfterDeserialize()
    {
        map.Clear();
        foreach (var e in entries)
            if (!string.IsNullOrEmpty(e.key))
                map[e.key] = e.value;
    }
}

[Serializable]
public class Triple<T1, T2, T3>
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;

    public Triple(T1 item1, T2 item2, T3 item3)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
    }
}
/*
 * 
 SaveManager.Instance.Save("playerHealth", 100);

int health = SaveManager.Instance.Load<int>("playerHealth");
Debug.Log($"Player Health: {health}");





SaveManager.Instance.Save("playerStats", 100, 50);

var stats = SaveManager.Instance.Load<Tuple<int, int>>("playerStats");
Debug.Log($"Health: {stats.Item1}, Mana: {stats.Item2}");





SaveManager.Instance.Save("playerPosition", 10f, 5f, -2f);

var position = SaveManager.Instance.Load<Triple<float, float, float>>("playerPosition");
Debug.Log($"X: {position.Item1}, Y: {position.Item2}, Z: {position.Item3}");



List<string> inventory = new List<string> { "Sword", "Shield", "Potion" };
SaveManager.Instance.SaveList("inventory", inventory);

List<string> loadedInventory = SaveManager.Instance.LoadList<string>("inventory");
Debug.Log($"Loaded Inventory: {string.Join(", ", loadedInventory)}");




List<int> playerScores = new List<int> { 10, 20, 30 };
List<string> playerNames = new List<string> { "Alice", "Bob", "Charlie" };
SaveManager.Instance.SaveList("playerData", playerScores, playerNames);

var playerData = SaveManager.Instance.Load<Tuple<List<int>, List<string>>>("playerData");
Debug.Log($"Scores: {string.Join(", ", playerData.Item1)}");
Debug.Log($"Names: {string.Join(", ", playerData.Item2)}");


List<int> enemyLevels = new List<int> { 1, 2, 3 };
List<float> enemyHealths = new List<float> { 100f, 150f, 200f };
List<string> enemyNames = new List<string> { "Goblin", "Orc", "Dragon" };

SaveManager.Instance.SaveList("enemyData", enemyLevels, enemyHealths, enemyNames);


var enemyData = SaveManager.Instance.Load<Triple<List<int>, List<float>, List<string>>>("enemyData");
Debug.Log($"Levels: {string.Join(", ", enemyData.Item1)}");
Debug.Log($"Healths: {string.Join(", ", enemyData.Item2)}");
Debug.Log($"Names: {string.Join(", ", enemyData.Item3)}");


 Tekli Değer (T)	Save("health", 100);	Load<int>("health");
İkili (Tuple<T1, T2>)	Save("stats", 100, 50);	Load<Tuple<int, int>>("stats");
Üçlü (Triple<T1, T2, T3>)	Save("pos", 10f, 5f, -2f);	Load<Triple<float, float, float>>("pos");
Liste (List<T>)	SaveList("inventory", list);	LoadList<string>("inventory");
İkili Liste (Tuple<List<T>, List<T>>)	SaveList("playerData", list1, list2);	Load<Tuple<List<int>, List<string>>>("playerData");
Üçlü Liste (Triple<List<T>, List<T>, List<T>>)	SaveList("enemyData", list1, list2, list3);	Load<Triple<List<int>, List<float>, List<string>>>("enemyData");
 
 * 
 */