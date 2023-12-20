using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameExit : MonoBehaviour
{
    [SerializeField]
    GameObject gameTitle; //게임 타이틀
    

    void Start()
    {
        gameTitle.SetActive(true);//처음 열린상태

    }
    public void OnGameStart() //게임 스타트 버튼 누르면 닫기
    {
        //게임 타이틀 닫기
        gameTitle.SetActive(false);//닫기

        //월드 인게임 씬 진입
        SceneManager.LoadScene("WorldMap");

    }
    public void Quit() //게임 나가기
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //play모드를 비활성화

        #else
            Application.Quit(); //exe파일로 게임실행할때 종료버튼 활성화 
        
        #endif
    }


    void Update()
    {

    }
}
