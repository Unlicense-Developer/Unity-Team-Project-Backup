using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPotion : MonoBehaviour
{
    public static GetPotion Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void GetP()
    {
        InventoryManager.Instance.AddItem("Potion");
        gameObject.SetActive(false);
    }
}
