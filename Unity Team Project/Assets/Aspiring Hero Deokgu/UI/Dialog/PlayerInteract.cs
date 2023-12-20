using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 3f; // ��ȣ�ۿ� ���� �Ÿ�
    public GameObject interactUiGroup; // ��ȣ�ۿ� UI ��� ����
    public GameObject npcInteractUI; // NPC ��ȣ�ۿ� UI ���
    public GameObject doorInteractUI; // �� ��ȣ�ۿ� UI ���
    public GameObject chestInteractUI; // ���� ��ȣ�ۿ� UI ���
    public GameObject entranceInteractUI; //  ��ȣ�ۿ� UI ���
    private GameObject currentInteractable; // ���� ������ ��ȣ�ۿ� ������ ��ü

    private void Update()
    {
        HandleInteraction();
    }

    private void OnTriggerEnter(Collider other)
    {
        interactUiGroup.SetActive(true); // ��� ��ȣ�ۿ��� ���۵� �� UI �׷� Ȱ��ȭ
        if (other.CompareTag("NPC"))
        {
            currentInteractable = other.gameObject;
            DisableOtherInteractUIs(npcInteractUI); // NPC UI Ȱ��ȭ�ϰ� ������ ��Ȱ��ȭ
        }
        else if (other.CompareTag("Door"))
        {
            currentInteractable = other.gameObject;
            DisableOtherInteractUIs(doorInteractUI); // �� UI Ȱ��ȭ�ϰ� ������ ��Ȱ��ȭ
        }
        else if (other.CompareTag("Chest"))
        {
            currentInteractable = other.gameObject;
            DisableOtherInteractUIs(chestInteractUI); // ���� UI Ȱ��ȭ�ϰ� ������ ��Ȱ��ȭ
        }
        else if (other.CompareTag("Entrance"))
        {
            currentInteractable = other.gameObject;
            DisableOtherInteractUIs(entranceInteractUI); // ���� UI Ȱ��ȭ�ϰ� ������ ��Ȱ��ȭ
        }
    }
    private void DisableOtherInteractUIs(GameObject activeUI)
    {
        // ���� ��ȣ�ۿ� UI�� �����ϰ� ��� ��ȣ�ۿ� UI ��Ȱ��ȭ
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
            interactUiGroup.SetActive(false); // NPC UI ��Ȱ��ȭ
            /*npcInteractUI.SetActive(false); // NPC UI ��Ȱ��ȭ
            doorInteractUI.SetActive(false); // �� UI ��Ȱ��ȭ
            chestInteractUI.SetActive(false); // ���� UI ��Ȱ��ȭ
            entranceInteractUI.SetActive(false); // ���� UI ��Ȱ��ȭ*/
        }
    }

    private void HandleInteraction()
    {
        // 'F' Ű�� ���� ���� ��ȣ�ۿ� ó��
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
                // ���� ��ȣ�ۿ� ó��
                ChestController chest = currentInteractable.GetComponent<ChestController>();
                chest.ToggleChest();
            }
            else if (currentInteractable.CompareTag("Entrance"))
            {
                // �Ա� ��ȣ�ۿ� ó��
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
            //Debug.Log("2. StartDialogue ����");
        }
    }
}
