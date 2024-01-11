using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsInput;

public class AchievementUI : MonoBehaviour
{
    [SerializeField]
    GameObject uiPanel;

    void Start()
    {
        uiPanel.SetActive(false);//ó�� ��������
    }
    void ToggleAchievement() //�κ��丮 ����,�ݱ�
    {
        if (WinInput.GetKeyDown(KeyCode.J))
        {
            uiPanel.SetActive(!uiPanel.activeSelf);
        }
    } 


    void Update()
    {
        ToggleAchievement();
    }
}
