using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPlayerData : MonoBehaviour
{
    void Start()
    {
        PlayerData.instance.LoadPlayerTransform(gameObject);
    }

    void OnDestroy()
    {
        PlayerData.instance.SavePlayerTransform(gameObject.transform.position, gameObject.transform.rotation);
    }
}
