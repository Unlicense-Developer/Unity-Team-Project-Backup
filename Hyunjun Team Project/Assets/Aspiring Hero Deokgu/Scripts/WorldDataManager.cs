using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Dungeon;

public class WorldDataManager : MonoBehaviour
{
    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.currentScene = SceneManager.GetActiveScene().name;

        // "Player" 태그를 가진 오브젝트 찾기
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            // 플레이어 위치와 회전 정보 저장
            var playerData = PlayerData.Instance;
            if (playerData != null)
            {
                playerData.SavePlayerTransform(playerObject.transform.position, playerObject.transform.rotation);
            }
        }

        // 다른 데이터 저장
        data.inventoryItems = InventoryManager.Instance.GetItems();
        data.gold = InventoryManager.Instance.GetGold();
        data.achievementData = GetAchievementData();

        SaveSystem.SaveGame(data);
    }


    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data != null)
        {
            //SceneManager.LoadScene(data.currentScene);
            LoadingSceneManager.Instance.StartLoadScene(data.currentScene);

            // "Player" 태그를 가진 오브젝트를 찾아서 위치와 회전 정보 로드
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                var playerData = PlayerData.Instance;
                if (playerData != null)
                {
                    playerData.LoadPlayerTransform(playerObject);
                }
            }

            // 다른 데이터 로드
            InventoryManager.Instance.SetItems(data.inventoryItems);
            InventoryManager.Instance.SetGold(data.gold);
            SetAchievementData(data.achievementData);
        }
    }

    private Dictionary<string, AchievementData> GetAchievementData()
    {
        var achievements = new Dictionary<string, AchievementData>();
        foreach (var achievement in FindObjectsOfType<AchievementBase>())
        {
            achievements[achievement.name] = achievement.GetAchievementData();
        }
        return achievements;
    }

    private void SetAchievementData(Dictionary<string, AchievementData> achievementData)
    {
        foreach (var achievement in FindObjectsOfType<AchievementBase>())
        {
            if (achievementData.TryGetValue(achievement.name, out var data))
            {
                achievement.SetAchieveValue(data.currentValue);
            }
        }
    }
}