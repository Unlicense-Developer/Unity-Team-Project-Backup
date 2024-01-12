using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPlayerData : MonoBehaviour
{
    void Start()
    {
        InventoryManager.Instance.LoadData();
        PlayerData.Instance.LoadPlayerTransform(gameObject);
    }

    void OnDestroy()
    {
        InventoryManager.Instance.SaveData();
        PlayerData.Instance.SavePlayerTransform(gameObject.transform.position, gameObject.transform.rotation);
    }
}
