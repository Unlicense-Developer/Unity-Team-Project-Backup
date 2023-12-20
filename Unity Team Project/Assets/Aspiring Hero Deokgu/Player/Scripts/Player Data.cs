using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    Vector3 playerPos = new Vector3( 1.0f, 0.2f, -9.35f);
    [SerializeField]
    Quaternion playerRot = Quaternion.Euler(Vector3.zero);

    int goldData = 1000;
    List<Item> invenData = new List<Item>();

    public static PlayerData instance = null;

    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // 씬 전환되더라도 파괴되지 않게 함
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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
        invenData.Add(ItemDataManager.instance.GetItem(itemName));
    }

    public void AddGold(int gold)
    {
        goldData += gold;
    }
}
