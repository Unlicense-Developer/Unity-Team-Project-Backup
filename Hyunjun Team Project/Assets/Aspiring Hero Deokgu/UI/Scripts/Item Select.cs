using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelect : MonoBehaviour
{
    public void Click(string type)
    {
        if ( type == "Shop")
            ShopManager.Instance.SelectItem(gameObject);
        else if ( type == "Inventory" )
            InventoryManager.Instance.SelectItem(gameObject);
    }
}
