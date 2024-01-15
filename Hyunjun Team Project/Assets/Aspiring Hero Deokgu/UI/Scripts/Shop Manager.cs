using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    GameObject select_Shopitem;

    [SerializeField] private GameObject shop_UI;
    [SerializeField] private Transform shopContent;
    [SerializeField] private GameObject select_Frame;
    [SerializeField] private GameObject shopSlotPrefab;

    public List<Item> shopItems = new List<Item>();

    //ΩÃ±€≈Ê
    public static ShopManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
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
        if ( select_Shopitem != null )
            select_Frame.transform.position = select_Shopitem.transform.position;
    }

    public void UpdateShopItem()
    {
        foreach (Transform item in shopContent)
        {
            Destroy(item.gameObject);
        }

        select_Shopitem = null;
        select_Frame.SetActive(false);

        foreach (Item item in shopItems)
        {
            GameObject itemSlot = Instantiate(shopSlotPrefab, shopContent.transform);

            itemSlot.transform.Find("Image_item").GetComponent<Image>().sprite = item.icon;
            itemSlot.transform.Find("Item Name").GetComponent<TMP_Text>().text = item.itemName;
            itemSlot.transform.Find("Item Value").Find("Gold Text").GetComponent<TMP_Text>().text = item.value.ToString();
        }
    }

    public Item GetSelectItem()
    {
        return ItemDataManager.Instance.GetItem(select_Shopitem.transform.Find("Image_item").GetComponent<Image>().sprite.name);
    }

    public void SelectItem(GameObject item)
    {
        select_Shopitem = item;
        Debug.Log(select_Shopitem.transform.Find("Image_item").GetComponent<Image>().sprite.name + " º±≈√");
        select_Frame.SetActive(true);
    }

    public void BuyItem()
    {
        if (InventoryManager.Instance.GetGold() < GetSelectItem().value)
            return;

        if (select_Shopitem == null)
            return;

        InventoryManager.Instance.AddItem(select_Shopitem);
        InventoryManager.Instance.UpdateInven();
        InventoryManager.Instance.AddGold(-GetSelectItem().value);
    }

    public void ActivateUI()
    {
        if (InventoryManager.Instance.GetUIState().activeSelf)
            InventoryManager.Instance.GetUIState().SetActive(false);

        shop_UI.SetActive(!shop_UI.activeSelf);
    }

    public GameObject GetUIState()
    {
        return shop_UI;
    }
}
