using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPlayerData : MonoBehaviour
{
    void Start()
    {
        PlayerData.Instance.LoadPlayerTransform(gameObject);
    }

    void OnDestroy()
    {
        PlayerData.Instance.SavePlayerTransform(gameObject.transform.position, gameObject.transform.rotation);
    }
}
