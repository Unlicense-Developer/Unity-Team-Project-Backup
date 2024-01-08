using System.Collections.Generic;
using UnityEngine;

public class NPCInteractionManager : MonoBehaviour
{
    public static NPCInteractionManager Instance { get; private set; }

    public float interactionRange = 3.0f; // 플레이어와 NPC 간 상호작용 가능 거리
    private Transform playerTransform;
    private List<NPCController> npcs = new List<NPCController>();

    public List<NPCController> GetNPCs()
    {
        return npcs;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        foreach (var npc in npcs)
        {
            float distance = Vector3.Distance(playerTransform.position, npc.transform.position);
            bool isInRange = distance < interactionRange;
            npc.SetInteractionUI(isInRange);
        }
    }

    public void RegisterNPC(NPCController npc)
    {
        if (!npcs.Contains(npc))
        {
            npcs.Add(npc);
        }
    }

    public void UnregisterNPC(NPCController npc)
    {
        npcs.Remove(npc);
    }
}
