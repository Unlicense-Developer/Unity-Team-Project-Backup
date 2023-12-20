using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    GameObject select_Shopitem;

    [SerializeField] private Transform shopContent;
    [SerializeField] private GameObject select_Frame;
    [SerializeField] private GameObject shopSlotPrefab;

    public List<Item> shopItems = new List<Item>();

    //ΩÃ±€≈Ê
    public static ShopManager instance = null;
    public static ShopManager Instance
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
        foreach (Item item in shopItems)
        {
            GameObject itemSlot = Instantiate(shopSlotPrefab, shopContent.transform);

            itemSlot.transform.Find("Image_item").GetComponent<Image>().sprite = item.icon;
            Transform test = itemSlot.transform.Find("Item Name");
            itemSlot.transform.Find("Item Name").GetComponent<TMP_Text>().text = item.itemName;
            itemSlot.transform.Find("Item Value").Find("Gold Text").GetComponent<TMP_Text>().text = item.value.ToString();
        }

        select_Shopitem = null;
        select_Frame.SetActive(false);
    }

    public void SelectItem(GameObject item)
    {
        select_Shopitem = item;
        Debug.Log(select_Shopitem.transform.Find("Image_item").GetComponent<Image>().sprite.name + " º±≈√");
        select_Frame.SetActive(true);
    }

    public void BuyItem()
    {
        if (select_Shopitem == null)
            return;

        InventoryManager.instance.AddItem(select_Shopitem);
        InventoryManager.instance.UpdateInven();
        InventoryManager.instance.gold -= ItemDataManager.instance.GetItem(select_Shopitem.transform.Find("Image_item").GetComponent<Image>().sprite.name).value;
    }
}
