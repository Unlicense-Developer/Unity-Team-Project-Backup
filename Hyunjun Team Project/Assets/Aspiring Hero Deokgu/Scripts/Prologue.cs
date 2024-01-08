using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KoreanTyper;
using UnityEngine.SceneManagement;


public class Prologue : MonoBehaviour
{
    public TextMeshProUGUI[] prologueTexts; // ���ѷα� �ؽ�Ʈ �迭
    public Image[] prologueImages; // ���ѷα� �̹��� �迭
    private string[] originalTexts; // ���� �ؽ�Ʈ�� �����ϴ� �迭
    private int currentTextIndex = 0; // ���� ǥ�� ���� �ؽ�Ʈ �ε���
    private bool isPrologueStarted = false;
    private bool isTyping = false; // Ÿ���� ������ �����ϴ� ����
    private bool isTypingComplete = false; // Ÿ���� �Ϸ� ���� ���� ����

    void Start()
    {
        InitializePrologueTexts();
        InitializePrologueImages();
    }

    void Update()
    {
        if (isPrologueStarted && currentTextIndex < prologueTexts.Length && Input.anyKeyDown)
        {
            if (isTyping)
            {
                // Ÿ���� ���̸� ��� ��� �ؽ�Ʈ�� ǥ��
                CompleteCurrentText();
            }
            else if (isTypingComplete && !isTyping)
            {
                // Ÿ������ �Ϸ�ǰ�, Ÿ���� ���� �ƴ� ���� ���� �ؽ�Ʈ�� �Ѿ
                NextText();
            }
        }
    }

    // ��� ���ѷα� �ؽ�Ʈ�� �ʱ�ȭ�ϴ� �Լ�
    private void InitializePrologueTexts()
    {
        originalTexts = new string[prologueTexts.Length];
        for (int i = 0; i < prologueTexts.Length; i++)
        {
            if (prologueTexts[i] != null)
            {
                originalTexts[i] = prologueTexts[i].text; // ���� �ؽ�Ʈ ����
                prologueTexts[i].gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Prologue text is null in prologueTexts array");
            }
        }
    }

    private void InitializePrologueImages()
    {
        // �̹����� ����ϴ�.
        foreach (var image in prologueImages)
        {
            if (image != null)
                image.gameObject.SetActive(false);
            else
                Debug.LogError("Prologue image is null in prologueImages array");
        }
    }

    // ���ѷα� ���� �Լ�
    public void PrologueStart()
    {
        if (!isPrologueStarted)
        {
            isPrologueStarted = true;
            // ù ��° �ؽ�Ʈ�� �̹��� ǥ��
            if (currentTextIndex == 0)
            {
                ShowCurrentTextAndImage();
            }
        }
    }

    private void CompleteCurrentText()
    {
        if (currentTextIndex < prologueTexts.Length && prologueTexts[currentTextIndex] != null)
        {
            TextMeshProUGUI currentText = prologueTexts[currentTextIndex];
            string originalText = originalTexts[currentTextIndex];
            currentText.text = originalText; // ���� �ؽ�Ʈ�� ������ ǥ��
            StopAllCoroutines(); // ��� �ڷ�ƾ ����
            isTyping = false; // Ÿ���� ���� ����
            isTypingComplete = true; // Ÿ���� �Ϸ� ���·� ����
        }
    }

    private void ShowCurrentTextAndImage()
    {
        if (currentTextIndex < prologueTexts.Length)
        {
            if (prologueTexts[currentTextIndex] != null)
                prologueTexts[currentTextIndex].gameObject.SetActive(true); // ���� �ؽ�Ʈ Ȱ��ȭ
            if (prologueImages[currentTextIndex] != null)
                prologueImages[currentTextIndex].gameObject.SetActive(true); // ���� �̹��� Ȱ��ȭ

            StartCoroutine(TypingRoutine());
        }
    }

    // Ÿ���� ȿ�� �ڷ�ƾ
    IEnumerator TypingRoutine()
    {
        if (currentTextIndex < prologueTexts.Length && prologueTexts[currentTextIndex] != null)
        {
            TextMeshProUGUI currentText = prologueTexts[currentTextIndex]; // ���� �ؽ�Ʈ ��ü ����
            string originalText = originalTexts[currentTextIndex]; // ���� �ؽ�Ʈ�� ���� ���� ����

            isTyping = true;
            isTypingComplete = false;
            int typingLength = originalText.GetTypingLength();
            for (int i = 0; i <= typingLength; i++)
            {
                currentText.text = originalText.Typing(i);
                yield return new WaitForSeconds(0.05f);
            }
            isTyping = false;
            isTypingComplete = true; // Ÿ���� �Ϸ� ���·� ����
        }
    }


    private void NextText()
    {
        // Ÿ������ �Ϸ�ǰ� Ÿ���� ���� �ƴ� ��쿡�� ���� �ؽ�Ʈ�� �Ѿ
        if (currentTextIndex < prologueTexts.Length - 1 && isTypingComplete && !isTyping)
        {
            // ���� �ؽ�Ʈ�� �̹����� ��Ȱ��ȭ
            DeactivateCurrentTextAndImage();

            currentTextIndex++; // �ε��� ����

            // ���� �ؽ�Ʈ�� �̹��� Ȱ��ȭ
            ActivateNextTextAndImage();

            // ���� �ؽ�Ʈ�� Ÿ���� ����
            StartCoroutine(TypingRoutine());
        }
    }

    private void DeactivateCurrentTextAndImage()
    {
        if (prologueTexts[currentTextIndex] != null)
            prologueTexts[currentTextIndex].gameObject.SetActive(false);
        if (prologueImages[currentTextIndex] != null)
            prologueImages[currentTextIndex].gameObject.SetActive(false);
    }

    private void ActivateNextTextAndImage()
    {
        if (prologueTexts[currentTextIndex] != null)
            prologueTexts[currentTextIndex].gameObject.SetActive(true);
        if (prologueImages[currentTextIndex] != null)
            prologueImages[currentTextIndex].gameObject.SetActive(true);
    }
}
