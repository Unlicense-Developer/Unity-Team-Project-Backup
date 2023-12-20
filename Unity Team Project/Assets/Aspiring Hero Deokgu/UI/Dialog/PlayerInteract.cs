using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 3f; // 상호작용 가능 거리
    public GameObject interactUiGroup; // 상호작용 UI 요소 묶음
    public GameObject npcInteractUI; // NPC 상호작용 UI 요소
    public GameObject doorInteractUI; // 문 상호작용 UI 요소
    public GameObject chestInteractUI; // 상자 상호작용 UI 요소
    public GameObject entranceInteractUI; //  상호작용 UI 요소
    private GameObject currentInteractable; // 현재 감지된 상호작용 가능한 객체

    private void Update()
    {
        HandleInteraction();
    }

    private void OnTriggerEnter(Collider other)
    {
        interactUiGroup.SetActive(true); // 모든 상호작용이 시작될 때 UI 그룹 활성화
        if (other.CompareTag("NPC"))
        {
            currentInteractable = other.gameObject;
            DisableOtherInteractUIs(npcInteractUI); // NPC UI 활성화하고 나머지 비활성화
        }
        else if (other.CompareTag("Door"))
        {
            currentInteractable = other.gameObject;
            DisableOtherInteractUIs(doorInteractUI); // 문 UI 활성화하고 나머지 비활성화
        }
        else if (other.CompareTag("Chest"))
        {
            currentInteractable = other.gameObject;
            DisableOtherInteractUIs(chestInteractUI); // 상자 UI 활성화하고 나머지 비활성화
        }
        else if (other.CompareTag("Entrance"))
        {
            currentInteractable = other.gameObject;
            DisableOtherInteractUIs(entranceInteractUI); // 입장 UI 활성화하고 나머지 비활성화
        }
    }
    private void DisableOtherInteractUIs(GameObject activeUI)
    {
        // 현재 상호작용 UI를 제외하고 모든 상호작용 UI 비활성화
        npcInteractUI.SetActive(activeUI == npcInteractUI);
        doorInteractUI.SetActive(activeUI == doorInteractUI);
        chestInteractUI.SetActive(activeUI == chestInteractUI);
        entranceInteractUI.SetActive(activeUI == entranceInteractUI);
    }

    public void DisableAllInteractUIs()
    {
        npcInteractUI.SetActive(false);
        doorInteractUI.SetActive(false);
        chestInteractUI.SetActive(false);
        entranceInteractUI.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC") || other.CompareTag("Door") || other.CompareTag("Chest") || other.CompareTag("Entrance"))
        {
            currentInteractable = null;
            interactUiGroup.SetActive(false); // NPC UI 비활성화
            /*npcInteractUI.SetActive(false); // NPC UI 비활성화
            doorInteractUI.SetActive(false); // 문 UI 비활성화
            chestInteractUI.SetActive(false); // 상자 UI 비활성화
            entranceInteractUI.SetActive(false); // 입장 UI 비활성화*/
        }
    }

    private void HandleInteraction()
    {
        // 'F' 키를 누를 때의 상호작용 처리
        if (Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            if (currentInteractable.CompareTag("NPC"))
            {
                StartNearestDialogue();
            }
            else if (currentInteractable.CompareTag("Door"))
            {
                DoorController door = currentInteractable.GetComponent<DoorController>();
                door.ToggleDoor();
            }
            else if (currentInteractable.CompareTag("Chest"))
            {
                // 상자 상호작용 처리
                ChestController chest = currentInteractable.GetComponent<ChestController>();
                chest.ToggleChest();
            }
            else if (currentInteractable.CompareTag("Entrance"))
            {
                // 입구 상호작용 처리
                EntranceController entrance = currentInteractable.GetComponent<EntranceController>();
                entrance.Enter();
            }
        }
    }

    private void StartNearestDialogue()
    {
        float minDistance = Mathf.Infinity;
        NPCController nearestNPC = null;

        foreach (var npc in NPCInteractionManager.Instance.GetNPCs())
        {
            if (npc.interactionUI.activeSelf)
            {
                float distance = Vector3.Distance(transform.position, npc.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestNPC = npc;
                }
            }
        }

        if (nearestNPC != null)
        {
            nearestNPC.StartDialogue();
            //Debug.Log("2. StartDialogue 시작");
        }
    }
}
