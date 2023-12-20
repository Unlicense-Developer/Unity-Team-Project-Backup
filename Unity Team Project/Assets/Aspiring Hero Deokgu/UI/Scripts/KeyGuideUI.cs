using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGuideUI : MonoBehaviour
{
    [SerializeField]
    GameObject keyGuideUi;
    bool activekeyGuide = false;
    void Start()
    {
        keyGuideUi.SetActive(false);//처음 닫은상태
    }
    void OnKeyGuideUIKey()//키보드 가이드 열기,닫기
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            activekeyGuide = !activekeyGuide;
            keyGuideUi.SetActive(activekeyGuide);
        }
    }
    public void OnKeyGuideUI()//키보드 가이드 버튼 열기
    {
        keyGuideUi.SetActive(true);
    }
    public void OffKeyGuideUI()//키보드 가이드 버튼 닫기
    {
        keyGuideUi.SetActive(false);
    }

    void Update()
    {
        OnKeyGuideUIKey();
    }
}



