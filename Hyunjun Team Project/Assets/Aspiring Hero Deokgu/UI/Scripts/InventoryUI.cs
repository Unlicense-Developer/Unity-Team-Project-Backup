using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            if (SceneManager.GetActiveScene().name == "WorldMap" || SceneManager.GetActiveScene().name == "Dungeon")
            {
                if (!invenPanel.activeSelf)
                    InventoryManager.Instance.UpdateInven();

                invenPanel.SetActive(!invenPanel.activeSelf);
            }
        }
    }

    void Update()
    {
        ToggleInventory();
    }

}





