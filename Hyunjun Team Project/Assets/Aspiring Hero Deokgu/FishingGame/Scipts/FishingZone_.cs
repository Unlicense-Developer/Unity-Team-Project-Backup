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

    public Collider zone1;
    public Collider zone2;
    public bool fishMode = false;


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

            if (this.gameObject.tag == "zone1")
            {
                FishingText.Instance.TutoA();
            }
            else if (this.gameObject.tag == "zone2")
            {
                FishingText.Instance.TutoB();
            }
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
}
