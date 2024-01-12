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

        // "Player" �±׸� ���� ������Ʈ ã��
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            // �÷��̾� ��ġ�� ȸ�� ���� ����
            var playerData = PlayerData.Instance;
            if (playerData != null)
            {
                playerData.SavePlayerTransform(playerObject.transform.position, playerObject.transform.rotation);
            }
        }

        // �ٸ� ������ ����
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

            // "Player" �±׸� ���� ������Ʈ�� ã�Ƽ� ��ġ�� ȸ�� ���� �ε�
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                var playerData = PlayerData.Instance;
                if (playerData != null)
                {
                    playerData.LoadPlayerTransform(playerObject);
                }
            }

            // �ٸ� ������ �ε�
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