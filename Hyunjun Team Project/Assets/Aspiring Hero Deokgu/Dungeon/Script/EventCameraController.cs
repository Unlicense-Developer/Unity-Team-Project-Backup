using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Dungeon;


public class EventCameraController : MonoBehaviour
{
    public static EventCameraController Instacne;

    public Camera mainCamera; // ���� ����ϴ� ī�޶�
    public Camera eventCamera; // �̺�Ʈ ī�޶�
    public Camera eventCamera_2; // �̺�Ʈ ī�޶� 2

    private bool isSpecialActive = false;

    private void Awake()
    {
        if (Instacne == null) Instacne = this;
    }

    private void Start()
    {
        eventCamera.gameObject.SetActive(false);
        eventCamera_2.gameObject.SetActive(false);
    }

    public void EventOn()
    {
        StartCoroutine(EvnetCamera1Effect());
        TextGUIManager.Instance.EventCameraTextA();
    }

    public void OtherEvnetOn()
    {
        StartCoroutine(EvnetCamera2Effect());
        TextGUIManager.Instance.EventCameraTextB();
    }


    IEnumerator EvnetCamera1Effect()
    {
        // ������ ����
        // ����: PlayerController ��ũ��Ʈ���� �÷��̾��� �������� ���� �� �ֽ��ϴ�.
        // playerController.DisableMovement();

        if (!isSpecialActive)
        {
            isSpecialActive = true;

            // Ư�� ī�޶� Ȱ��ȭ
            eventCamera.gameObject.SetActive(true);

            // DOTween�� ����Ͽ� Ư�� ī�޶� ��鸲 ȿ�� �ֱ�
            eventCamera.transform.DOShakePosition(10f, strength: 0.2f, vibrato: 13, randomness: 90);

            yield return new WaitForSeconds(12f);

            mainCamera.gameObject.SetActive(true);      // ���� ����ϴ� ī�޶�� ����
            eventCamera.gameObject.SetActive(false);    //�̺�Ʈ ī�޶� ��Ȱ��ȭ

            isSpecialActive = false;


            // ������ �ٽ� ���
            // ����: playerController.EnableMovement();
        }
    }

    IEnumerator EvnetCamera2Effect()
    {
        // ������ ����
        // ����: PlayerController ��ũ��Ʈ���� �÷��̾��� �������� ���� �� �ֽ��ϴ�.
        // playerController.DisableMovement();

        if (!isSpecialActive)
        {
            isSpecialActive = true;

            // Ư�� ī�޶� Ȱ��ȭ
            eventCamera_2.gameObject.SetActive(true);

            // DOTween�� ����Ͽ� Ư�� ī�޶� ��鸲 ȿ�� �ֱ�
            eventCamera_2.transform.DOShakePosition(10f, strength: 0.1f, vibrato: 10, randomness: 90);

            yield return new WaitForSeconds(12f);

            mainCamera.gameObject.SetActive(true);      // ���� ����ϴ� ī�޶�� ����
            eventCamera_2.gameObject.SetActive(false);    //�̺�Ʈ ī�޶� ��Ȱ��ȭ

            isSpecialActive = false;

            // ������ �ٽ� ���
            // ����: playerController.EnableMovement();
        }
    }
}