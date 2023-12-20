using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsInput;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    GameObject invenPanel;

    void Start()
    {
        invenPanel.SetActive(false);//처음 닫은상태

    }
    void ToggleInventory() //인벤토리 열기,닫기
    {
        if (WinInput.GetKeyDown(KeyCode.I))
        {
            if (!invenPanel.activeSelf)
                InventoryManager.instance.UpdateInven();

            invenPanel.SetActive(!invenPanel.activeSelf);
        }
    }

    void Update()
    {
        ToggleInventory();
    }

}





