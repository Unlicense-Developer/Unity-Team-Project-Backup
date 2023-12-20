using System.Collections;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public GameObject interactionUI;
    public DialogNPC npcDialogue; // NPC ��ȭ ��ũ��Ʈ ����

    private void OnEnable()
    {
        StartCoroutine(RegisterWithManager());
    }

    private IEnumerator RegisterWithManager()
    {
        while (NPCInteractionManager.Instance == null)
        {
            yield return null; // NPCInteractionManager�� �ν��Ͻ��� ������ ������ ���
        }
        NPCInteractionManager.Instance.RegisterNPC(this);
    }

    private void OnDisable()
    {
        if (NPCInteractionManager.Instance != null)
        {
            NPCInteractionManager.Instance.UnregisterNPC(this);
        }
    }

    public void SetInteractionUI(bool active)
    {
        interactionUI.SetActive(active);
    }

    public void StartDialogue()
    {
        if (interactionUI.activeSelf) // interactionUI�� Ȱ��ȭ�� ��쿡�� ��ȭ ����
        {
            StartCoroutine(npcDialogue.Talk());
            //Debug.Log("3. StartCoroutine(npcDialogue.Talk()) ����");
        }
    }
}
