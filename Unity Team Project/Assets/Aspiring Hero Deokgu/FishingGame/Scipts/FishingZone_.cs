using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FishingZoneType
{
    ZONE1,
    ZONE2
}

public class FishingZone_ : MonoBehaviour
{
    PlayerController.ThirdPersonController controller;

    public Collider zone1;
    public Collider zone2;
    public bool fishMode = false;

    /*
    public GameObject fishingZone_1; // ���� ����1
    public GameObject fishingZone_2; // ���� ����2
    */

    public Collider GetCollider(FishingZoneType ZoneType)
    {
        switch (ZoneType)
        {
            case FishingZoneType.ZONE1:
                return zone1;
            case FishingZoneType.ZONE2:
                return zone2;
            default:
                return null;
        }
    }

    public bool playerCanFish = false; // �÷��̾ ���ø� �� �� �ִ��� ����


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("���� ������ �����߽��ϴ�."); // ����� �α�: �÷��̾ ���� ������ ������
            playerCanFish = true;
            fishMode = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("���� ������ ������ϴ�."); // ����� �α�: �÷��̾ ���� ������ ���
            playerCanFish = false;
            fishMode = false;
        }
    }

    public bool PlayerCanFish()
    {
        return playerCanFish;
    }

    /*
    public bool PlayerCanFish
    {
        get
        {
            return playerCanFish;
        }
    }
    */

    //public string PlayerCanFish
    //{
    //    get
    //    {
    //          if(playerCanFish)
    //              return "zone1";
    //          else
    //              return "zone2";
    //    }
    //}
}
