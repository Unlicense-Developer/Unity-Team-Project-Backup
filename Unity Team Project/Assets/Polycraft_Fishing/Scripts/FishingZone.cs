using UnityEngine;
using System.Collections;

public class FishingZone : MonoBehaviour
{
    bool playerCanFish = false; // 플레이어가 낚시를 할 수 있는지 여부

    public Color gizmoColor = new Color(1, 0, 0, 0.5f); // 기즈모 색상 설정

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("낚시 지역에 진입했습니다."); // 디버그 로그: 플레이어가 낚시 지역에 진입함
            playerCanFish = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("낚시 지역을 벗어났습니다."); // 디버그 로그: 플레이어가 낚시 지역을 벗어남
            playerCanFish = false;
        }
    }

    public bool PlayerCanFish()
    {
        return playerCanFish;
    }

    void OnDrawGizmosSelected()
    {
        // 해당 위치에 반투명한 파란색 큐브 그리기
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
