using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public string currentScene;
    public Vector3 playerPosition;
    public List<Item> inventoryItems;
    public int gold;
    public Dictionary<string, AchievementData> achievementData; // 업적 데이터 추가

    public SaveData()
    {
        inventoryItems = new List<Item>();
        achievementData = new Dictionary<string, AchievementData>();
    }
}

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/save.json";

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.LogError("Save file not found in " + savePath);
            return null;
        }
    }
}
