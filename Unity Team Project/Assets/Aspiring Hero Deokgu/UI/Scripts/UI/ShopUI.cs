using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField]
    GameObject shopPanel;
    bool activeShop = false;

    void Start()
    {
        shopPanel.SetActive(activeShop);//처음 닫은상태

    }
    public void OnShop()//상점 버튼으로 열기 
    {
        shopPanel.SetActive(true);

    }
    public void OffShop()//상점 버튼으로 닫기
    {
        shopPanel.SetActive(false);
    }
    void Update()
    {

    }
}
