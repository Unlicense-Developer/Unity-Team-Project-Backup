using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    [SerializeField]
    SerializableDictionary<string, Item> itemDatas;

    //ΩÃ±€≈Ê
    public static ItemDataManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    public Item GetItem(string item)
    {
        return itemDatas[item];
    }
}
