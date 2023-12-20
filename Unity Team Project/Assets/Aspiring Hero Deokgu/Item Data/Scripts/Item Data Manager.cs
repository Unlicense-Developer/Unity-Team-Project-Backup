using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    [SerializeField]
    SerializableDictionary<string, Item> itemDatas;

    //ΩÃ±€≈Ê
    public static ItemDataManager instance = null;
    public static ItemDataManager Instance
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

            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    public Item GetItem(string item)
    {
        return itemDatas[item];
    }
}
