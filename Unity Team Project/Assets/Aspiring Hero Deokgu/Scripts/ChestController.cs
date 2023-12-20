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
            // ���� ���� ���� �ִϸ��̼� �� ����
            Debug.Log("Chest opened.");
        }
        else
        {
            // ���� ���� �ݱ� �ִϸ��̼� �� ����
            Debug.Log("Chest closed.");
        }
    }
}