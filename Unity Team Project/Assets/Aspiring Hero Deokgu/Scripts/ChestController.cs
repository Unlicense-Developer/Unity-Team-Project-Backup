using UnityEngine;

public class ChestController : MonoBehaviour
{
    public GameObject interactionUI;
    public bool isOpen = false;

    public void ToggleChest()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            // 보물 상자 열기 애니메이션 및 로직
            Debug.Log("Chest opened.");
        }
        else
        {
            // 보물 상자 닫기 애니메이션 및 로직
            Debug.Log("Chest closed.");
        }
    }
}