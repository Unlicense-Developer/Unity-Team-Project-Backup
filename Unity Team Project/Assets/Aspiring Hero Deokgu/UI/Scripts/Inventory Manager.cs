using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WindowsInput;

public class InventoryManager : MonoBehaviour
{
    public int gold;
    GameObject select_Item;

    [SerializeField] private TMP_Text sellPriceText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject select_Frame;
    [SerializeField] private GameObject invenSlotPrefab;

    List<Item> inven = new List<Item>();

    //싱글톤
    public static InventoryManager instance = null;
    public static InventoryManager Instance
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
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }

        inven = PlayerData.instance.GetInvenData();
        gold = PlayerData.instance.GetGold();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = gold.ToString();

        if (select_Item != null)
            select_Frame.transform.position = select_Item.transform.position;
    }

    void OnDestroy()
    {
        PlayerData.instance.SaveInvenData(inven);
        PlayerData.instance.SaveGold(gold);
    }

    public void AddItem(string item)
    {
        inven.Add(ItemDataManager.instance.GetItem(item));
    }

    public void AddItem(GameObject item)
    {
        inven.Add(ItemDataManager.instance.GetItem(item.transform.Find("Image_item").GetComponent<Image>().sprite.name));
    }

    public void RemoveItem(string item)
    {
        inven.Remove(ItemDataManager.instance.GetItem(item));
    }

    public void UpdateInven()
    {
        foreach( Transform item in content)
        {
            Destroy(item.gameObject);
        }

        select_Item = null;
        select_Frame.SetActive(false);

        foreach (Item item in inven)
        {
            GameObject itemSlot = Instantiate(invenSlotPrefab, content.transform);
            Image itemIcon = itemSlot.transform.Find("Image_item").GetComponent<Image>();

            itemIcon.sprite = item.icon;
        }
    }

    public void SelectItem( GameObject item)
    {
        //select_Item = ItemDataManager.instance.GetItem(item.transform.Find("Image_item").GetComponent<Image>().sprite.name);
        select_Item = item;
        Debug.Log(select_Item.transform.Find("Image_item").GetComponent<Image>().sprite.name + " 선택");
        sellPriceText.text = GetSelectItem().value.ToString();
        select_Frame.SetActive(true);
    }

    public Item GetSelectItem()
    {
        return ItemDataManager.instance.GetItem(select_Item.transform.Find("Image_item").GetComponent<Image>().sprite.name);
    }

    public void UseSelectItem()
    {
        if (select_Item == null)
            return;

        if (GetSelectItem().type == ItemType.Potion)
        {
            Debug.Log("포션을 사용했습니다.");
            RemoveItem(select_Item.transform.Find("Image_item").GetComponent<Image>().sprite.name);
            UpdateInven();
        }
    }

    public void DeleteSelectItem()
    {
        if (select_Item == null) 
            return;

        RemoveItem(select_Item.transform.Find("Image_item").GetComponent<Image>().sprite.name);
        UpdateInven();
    }

    public void SellItem()
    {
        if (select_Item == null)
            return;

        DeleteSelectItem();
        gold += GetSelectItem().value / 2;
    }
}
