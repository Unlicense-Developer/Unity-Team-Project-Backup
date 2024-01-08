using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUI : MonoBehaviour
{
    [SerializeField]
    GameObject worldUiList; //월드 UI

    void Start()
    {
        worldUiList.SetActive(false); //처음 닫은상태
    }

    public void OnWorldUI() //월드UI 열기,닫기
    {
        bool isActive = worldUiList.activeSelf;
        worldUiList.SetActive(!isActive);
    }
}
