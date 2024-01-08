using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject interactionUI; // 문과 상호작용할 때 표시할 UI
    public Animator doorAnimator; // 문의 애니메이션을 제어할 Animator

    private bool isOpen = false; // 문이 열렸는지의 상태

    private void OnEnable()
    {
        // NPCInteractionManager에 문 등록 (선택적)
        // NPCInteractionManager.Instance.RegisterDoor(this);
    }

    private void OnDisable()
    {
        // NPCInteractionManager에 문 등록 해제 (선택적)
        // NPCInteractionManager.Instance.UnregisterDoor(this);
    }

    public void SetInteractionUI(bool active)
    {
        interactionUI.SetActive(active);
    }

    // 문을 열거나 닫는 메서드
    public void ToggleDoor()
    {
        isOpen = !isOpen; // 상태를 반전시킴
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsOpen", isOpen); // Animator에 상태 전달
        }

        Debug.Log(isOpen ? "Door opened." : "Door closed.");
    }

    // 문을 직접 열기 위한 메서드
    public void OpenDoor()
    {
        if (!isOpen) // 문이 닫혀있을 때만 실행
        {
            ToggleDoor();
        }
    }

    // 문을 직접 닫기 위한 메서드
    public void CloseDoor()
    {
        if (isOpen) // 문이 열려있을 때만 실행
        {
            ToggleDoor();
        }
    }
}
