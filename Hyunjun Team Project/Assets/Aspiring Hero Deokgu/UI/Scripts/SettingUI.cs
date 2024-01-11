using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    [SerializeField]
    GameObject settingPanel;
    void Start()
    {
        //settingPanel.SetActive(false);
    }
    public void Onsetting()//인벤토리 버튼으로 열기 
    {
        settingPanel.SetActive(true);

    }
    public void Offsetting()//인벤토리 버튼으로 닫기
    {
        settingPanel.SetActive(false);
    }

    void Update()
    {

    }
}
