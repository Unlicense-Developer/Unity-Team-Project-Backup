using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    public void OpenUI(string uiType)
    {
        if( uiType == "Inventory")
        {
            InventoryManager.Instance.UpdateInven();
            InventoryManager.Instance.ActivateUI();
        }
        else if( uiType == "Achievement")
        {
            AchievementManager.Instance.ActivateUI();
        }

    }
}
