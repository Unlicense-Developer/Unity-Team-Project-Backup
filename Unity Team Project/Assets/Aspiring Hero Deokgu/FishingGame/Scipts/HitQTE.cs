using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitQTE : MonoBehaviour
{
    //��Ÿ�� QTE ���� ����
    PlayerController.ThirdPersonController Controller;
    PlayerFishingController FC;
    public int requiredPresses = 30; // �ʿ��� ��Ÿ Ƚ��
    public Slider HitqteSlider; // ��Ÿ �����̴�
    public GameObject qtePanel; // QTE �г�
    private int currentPresses = 0; // ��������� ��Ÿ Ƚ��
    private float qteTimer = 10.0f; // QTE ���� �ð�
    private bool isHitQTEActive = true; // QTE Ȱ��ȭ ����
    Animator anim;

    private void Start()
    {
        Controller = GetComponent<PlayerController.ThirdPersonController>();
        FC = GetComponent<PlayerFishingController>();
    }

    public void HitQTEAction()
    {

        anim.SetBool("ReelIn", true);  //���� ���� �ִϸ��̼��� ����
        qtePanel.SetActive(true);   //QTE��� Ȱ��ȭ
        HitqteSlider.gameObject.SetActive(true);    //QTE�����̴� Ȱ��ȭ

        if (FC.isFishingActive)
        {
            // J Ű�� ������ �� QTE�� �������� ���
            if (Input.GetKeyDown(KeyCode.J))
            {
                anim.SetBool("ReelIn", true);
                currentPresses++; // �Էµ� ��Ÿ Ƚ�� ����
                Debug.Log(currentPresses);
                HitqteSlider.value = (float)currentPresses / requiredPresses; // ��Ÿ Ƚ���� ����Ͽ� �����̴� �� ����
                print("c : " + currentPresses + " r : " + requiredPresses);
                // �ʿ��� ��Ÿ Ƚ���� �����ϸ� ����
                if (currentPresses >= requiredPresses)
                {
                    anim.SetBool("ReelIn", false);
                    anim.SetInteger("Catch", 0);
                    hideHitQTEPanel(); // QTE ���� �� �г� ����
                    ResetHitQTE(); // QTE �ʱ�ȭ
                    Debug.Log("����⸦ ��ҽ��ϴ�!");
                    FC.isFishing = false;
                    anim.SetBool("IsFishing", false);
                    Invoke("DisableFishingPole", 2.0f);
                }
            }

            // �ð��� ���ҿ� ���������� �����ϰ� �� ���� ������ ����
            qteTimer -= Time.deltaTime;
            if (qteTimer <= 0)
            {
                Debug.Log("�ð� �ʰ� - QTE ����!");
                ResetHitQTE(); // �ð� �ʰ� �� QTE �ʱ�ȭ
                hideHitQTEPanel();
                hideSlider();
                anim.SetBool("IsFishing", false);
                anim.SetBool("ReelIn", false);
                anim.SetInteger("Catch", 1);
                FC.isFishing = false;
                Invoke("DisableFishingPole", 2.0f);
            }
        }
    }

    void hideHitQTEPanel()
    {
        // QTE �г� ��Ȱ��ȭ �� 1�� �Ŀ� �����̴� ����
        qtePanel.SetActive(false);
        Invoke("hideSlider", 1.0f);
    }

    void hideSlider()
    {
        // �����̴� �����
        HitqteSlider.gameObject.SetActive(false);
    }

    void ResetHitQTE()
    {
        // QTE�� �Էµ� ���� �ʱ�ȭ�ϰ� ���� ������ �غ�
        HitqteSlider.value = 0;
        currentPresses = 0;
        qteTimer = 10.0f;
        FC.timerToCatch = 0;
        FC.isFishingActive = false; // QTE ��Ȱ��ȭ
    }


}
