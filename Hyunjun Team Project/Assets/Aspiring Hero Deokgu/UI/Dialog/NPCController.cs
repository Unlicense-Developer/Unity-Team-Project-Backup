using System.Collections;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public GameObject interactionUI;
    public DialogNPC npcDialogue; // NPC 대화 스크립트 참조

    private void OnEnable()
    {
        StartCoroutine(RegisterWithManager());
    }

    private IEnumerator RegisterWithManager()
    {
        while (NPCInteractionManager.Instance == null)
        {
            yield return null; // NPCInteractionManager의 인스턴스가 설정될 때까지 대기
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
        if (interactionUI.activeSelf) // interactionUI가 활성화된 경우에만 대화 시작
        {
            StartCoroutine(npcDialogue.Talk());
            //Debug.Log("3. StartCoroutine(npcDialogue.Talk()) 시작");
        }
    }
}
