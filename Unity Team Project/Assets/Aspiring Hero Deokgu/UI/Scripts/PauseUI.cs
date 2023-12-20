using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    GameObject PausePanel;
    bool activePausePanel = false;
    void Start()
    {
        PausePanel.SetActive(activePausePanel);//처음 닫은상태
    }
    void OnPauseUIKey()//일시정지창 열기,닫기
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activePausePanel = !activePausePanel;
            PausePanel.SetActive(activePausePanel);
        }
    }

    public void ResumeUIKey() //일시정지창 버튼으로 닫기
    {
        PausePanel.SetActive(false);
        //게임화면 다시 진입

    }
    void Update()
    {
        OnPauseUIKey();
    }
}
