using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using WindowsInput;

public class InventoryManager : MonoBehaviour
{
    public int gold;
    GameObject select_Item;

    [SerializeField] private TMP_Text sellPriceText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text selectItemInfoName;
    [SerializeField] private TMP_Text selectItemInfoText;
    [SerializeField] private TMP_Text selectItemSellPrice;

    [SerializeField] private Transform content;
    [SerializeField] private GameObject inven_UI;
    [SerializeField] private GameObject selectItemInfoImage;
    [SerializeField] private GameObject select_Frame;
    [SerializeField] private GameObject invenSlotPrefab;
    [SerializeField] private GameObject selectItemInfo;

    List<Item> inven = new List<Item>();

    //싱글톤
    public static InventoryManager Instance { get; private set; }

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

    void Update()
    {
        goldText.text = gold.ToString();

        if (select_Item != null)
            select_Frame.transform.position = select_Item.transform.position;
    }

    private void OnEnable()
    {
        LoadData();
    }


    public void AddItem(string item)
    {
        inven.Add(ItemDataManager.Instance.GetItem(item));
    }

    public void AddItem(GameObject item)
    {
        inven.Add(ItemDataManager.Instance.GetItem(item.transform.Find("Image_item").GetComponent<Image>().sprite.name));
    }

    public void RemoveItem(string item)
    {
        inven.Remove(ItemDataManager.Instance.GetItem(item));
    }

    public void ActivateUI()
    {
        inven_UI.SetActive(true);
    }

    public int GetGold()
    {
        return gold;
    }

    public void SetGold(int setGold)
    {
        gold = setGold;
    }

    public void AddGold(int addGold)
    {
        gold += addGold;
    }

    public void UpdateInven()
    {
        foreach( Transform item in content)
        {
            Destroy(item.gameObject);
        }

        select_Item = null;
        select_Frame.SetActive(false);
        selectItemInfo.SetActive(false);

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
        sellPriceText.text = ( GetSelectItem().value / 2 ).ToString();
        select_Frame.SetActive(true);

        ActiveSelectItemInfo();
    }

    public Item GetSelectItem()
    {
        return ItemDataManager.Instance.GetItem(select_Item.transform.Find("Image_item").GetComponent<Image>().sprite.name);
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

        gold += GetSelectItem().value / 2;
        DeleteSelectItem();
    }

    // 인벤토리에 특정 아이템이 존재하는지 확인하는 메서드
    public bool HasItem(string itemName)
    {
        foreach (Item item in inven)
        {
            if (item.name == itemName)
                return true;
        }
        return false;
    }

    // 인벤토리 내의 모든 아이템을 반환하는 메서드
    public List<Item> GetItems()
    {
        return new List<Item>(inven);
    }

    // 인벤토리 아이템 목록을 설정하는 메서드
    public void SetItems(List<Item> items)
    {
        inven = new List<Item>(items);
        UpdateInven(); // 인벤토리 UI 업데이트
    }

    // 인벤토리 아이템 갯수 체크 메서드
    public int GetItemCount(string itemName)
    {
        int count = 0;
        foreach (Item item in inven)
        {
            if (item.name == itemName)
            {
                count++;
            }
        }
        return count;
    }

    void ActiveSelectItemInfo()
    {
        if( select_Frame.activeSelf)
        {
            selectItemInfo.SetActive(true);
            selectItemInfoName.text = GetSelectItem().itemName;
            selectItemInfoImage.GetComponent<Image>().sprite = GetSelectItem().icon;
            selectItemInfoText.text = GetSelectItem().itemInfo;
            selectItemSellPrice.text = "상점판매가 : " + (GetSelectItem().value / 2).ToString() + "골드";
        }
    }

    public void SaveData()
    {
        PlayerData.Instance.SaveInvenData(inven);
        PlayerData.Instance.SaveGold(gold);
    }

    public void LoadData()
    {
        inven = PlayerData.Instance.GetInvenData();
        gold = PlayerData.Instance.GetGold();
    }
}
