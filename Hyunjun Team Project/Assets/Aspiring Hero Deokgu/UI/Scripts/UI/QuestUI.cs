using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField]
    GameObject QuestPanel;
    bool activeQuest = false;

    void Start()
    {
        QuestPanel.SetActive(activeQuest);//처음 닫은상태

    }
    public void OnInventory()//퀘스트 버튼으로 열기 
    {
        QuestPanel.SetActive(true);

    }
    public void OffInventory()//퀘스트 버튼으로 닫기
    {
        QuestPanel.SetActive(false);
    }
}
