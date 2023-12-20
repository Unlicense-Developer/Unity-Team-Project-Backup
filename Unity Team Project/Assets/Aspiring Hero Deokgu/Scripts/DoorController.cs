using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject interactionUI; // ���� ��ȣ�ۿ��� �� ǥ���� UI
    public Animator doorAnimator; // ���� �ִϸ��̼��� ������ Animator

    private bool isOpen = false; // ���� ���ȴ����� ����

    private void OnEnable()
    {
        // NPCInteractionManager�� �� ��� (������)
        // NPCInteractionManager.Instance.RegisterDoor(this);
    }

    private void OnDisable()
    {
        // NPCInteractionManager�� �� ��� ���� (������)
        // NPCInteractionManager.Instance.UnregisterDoor(this);
    }

    public void SetInteractionUI(bool active)
    {
        interactionUI.SetActive(active);
    }

    // ���� ���ų� �ݴ� �޼���
    public void ToggleDoor()
    {
        isOpen = !isOpen; // ���¸� ������Ŵ
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsOpen", isOpen); // Animator�� ���� ����
        }

        Debug.Log(isOpen ? "Door opened." : "Door closed.");
    }

    // ���� ���� ���� ���� �޼���
    public void OpenDoor()
    {
        if (!isOpen) // ���� �������� ���� ����
        {
            ToggleDoor();
        }
    }

    // ���� ���� �ݱ� ���� �޼���
    public void CloseDoor()
    {
        if (isOpen) // ���� �������� ���� ����
        {
            ToggleDoor();
        }
    }
}
