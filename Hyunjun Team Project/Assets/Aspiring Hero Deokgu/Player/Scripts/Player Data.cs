using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    Vector3 playerPos = new Vector3(1.0f, 0.2f, -9.35f);
    [SerializeField]
    Quaternion playerRot = Quaternion.Euler(Vector3.zero);

    public int goldData = 1000;
    List<Item> invenData = new List<Item>();

    public static PlayerData Instance { get; private set; }

    // �÷��̾� ���� ������ ���� Ŭ����
    public class PlayerStatusData
    {
        public int CurrentHealth;
        public int BaseAttackDamage;
        public int AdditionalAttackDamage;
        public int BaseBreakDamage;
        public int AdditionalBreakDamage;
        public int Defense;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // �� ��ȯ�Ǵ��� �ı����� �ʰ� ��
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    public void SavePlayerTransform(Vector3 curPos, Quaternion curRot)
    {
        playerPos = curPos;
        playerRot = curRot;
    }

    public void LoadPlayerTransform(GameObject player)
    {
        player.transform.position = playerPos;
        player.transform.rotation = playerRot;
    }

    public void SaveInvenData(List<Item> curInven)
    {
        invenData = curInven;
    }

    public void SaveGold(int curGold)
    {
        goldData = curGold;
    }

    public List<Item> GetInvenData()
    {
        return invenData;
    }

    public int GetGold()
    {
        return goldData;
    }

    public void AddItemData(string itemName)
    {
        invenData.Add(ItemDataManager.Instance.GetItem(itemName));
    }

    public void DeleteItemData(string itemName)
    {
        invenData.Remove(ItemDataManager.Instance.GetItem(itemName));
    }

    public void AddGold(int gold)
    {
        goldData += gold;
    }

    public void SetGold(int gold)
    {
        goldData = gold;
    }

    public void SavePlayerStatus(int currentHealth, int baseAttack, int additionalAttack, int baseBreak, int additionalBreak, int defense)
    {
        // ���⿡ �÷��̾� ���� ���� ���� ����
    }

    public PlayerStatusData GetPlayerStatus()
    {
        // ���⿡ �÷��̾� ���� �ε� ���� ����
        return new PlayerStatusData();
    }
}
