using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelect : MonoBehaviour
{
    public void Click(string type)
    {
        if ( type == "Shop")
            ShopManager.instance.SelectItem(gameObject);
        else if ( type == "Inventory" )
            InventoryManager.instance.SelectItem(gameObject);
    }
}
