using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingFishTest : MonoBehaviour
{
    public bool delay; // 딜레이 여부
    public Vector2 TrandomMinMax = new Vector2(3, 7); // 랜덤한 딜레이 시간 범위
    public float randomTime; // 현재 랜덤 딜레이

    public Vector2 xMinMax = new Vector2(6850.522f, 6851.522f); // x 좌표 이동 범위
    public Vector2 zMinMax = new Vector2(-2175.035f, -2174.035f); // z 좌표 이동 범위
    public Vector3 randomPos; // 랜덤한 위치


    void Start()
    {
        // 초기 설정
        randomTime = Random.Range(1.5f, (int) TrandomMinMax.y);

        randomPos = transform.position;
        randomPos.x = Random.Range(xMinMax.x, xMinMax.y);
        randomPos.z = Random.Range(zMinMax.x, zMinMax.y);

        transform.position = randomPos;

        delay = true;
    }


    void Update()
    {
        if (delay)
        {
            // 딜레이 중일 때
            if (randomTime >= 0)
            {
                randomTime -= Time.deltaTime;
            }
            else
            {
                // 딜레이가 끝나면 새로운 랜덤한 위치와 각도로 설정
                randomTime = Random.Range((int) TrandomMinMax.x, (int) TrandomMinMax.y);
                if (randomTime < 1)
                {
                    randomTime = Random.Range((int) TrandomMinMax.x, (int) TrandomMinMax.y);
                }

                randomPos.x = Random.Range(xMinMax.x, xMinMax.y);
                randomPos.z = Random.Range(zMinMax.x, zMinMax.y);
                transform.position = randomPos;

                Vector3 euler = transform.eulerAngles;
                euler.y = Random.Range(0f, 360f);
                transform.eulerAngles = euler;

                // 애니메이션 트리거 활성화 및 딜레이 상태 전환
                transform.GetComponent<Animator>().SetBool("Jumping", true);
                delay = false;
            }
        }
    }

    public void EndAnimation()
    {
        // 애니메이션 종료 및 딜레이 상태 전환
        transform.GetComponent<Animator>().SetBool("Jumping", false);
        delay = true;
    }
}
