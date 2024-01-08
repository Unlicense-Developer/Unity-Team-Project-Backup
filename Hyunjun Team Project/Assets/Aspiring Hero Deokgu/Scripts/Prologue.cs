using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KoreanTyper;
using UnityEngine.SceneManagement;


public class Prologue : MonoBehaviour
{
    public TextMeshProUGUI[] prologueTexts; // 프롤로그 텍스트 배열
    public Image[] prologueImages; // 프롤로그 이미지 배열
    private string[] originalTexts; // 원본 텍스트를 저장하는 배열
    private int currentTextIndex = 0; // 현재 표시 중인 텍스트 인덱스
    private bool isPrologueStarted = false;
    private bool isTyping = false; // 타이핑 중인지 추적하는 변수
    private bool isTypingComplete = false; // 타이핑 완료 여부 추적 변수

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
                // 타이핑 중이면 즉시 모든 텍스트를 표시
                CompleteCurrentText();
            }
            else if (isTypingComplete && !isTyping)
            {
                // 타이핑이 완료되고, 타이핑 중이 아닐 때만 다음 텍스트로 넘어감
                NextText();
            }
        }
    }

    // 모든 프롤로그 텍스트를 초기화하는 함수
    private void InitializePrologueTexts()
    {
        originalTexts = new string[prologueTexts.Length];
        for (int i = 0; i < prologueTexts.Length; i++)
        {
            if (prologueTexts[i] != null)
            {
                originalTexts[i] = prologueTexts[i].text; // 원본 텍스트 저장
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
        // 이미지를 숨깁니다.
        foreach (var image in prologueImages)
        {
            if (image != null)
                image.gameObject.SetActive(false);
            else
                Debug.LogError("Prologue image is null in prologueImages array");
        }
    }

    // 프롤로그 시작 함수
    public void PrologueStart()
    {
        if (!isPrologueStarted)
        {
            isPrologueStarted = true;
            // 첫 번째 텍스트와 이미지 표시
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
            currentText.text = originalText; // 현재 텍스트를 완전히 표시
            StopAllCoroutines(); // 모든 코루틴 중지
            isTyping = false; // 타이핑 상태 해제
            isTypingComplete = true; // 타이핑 완료 상태로 설정
        }
    }

    private void ShowCurrentTextAndImage()
    {
        if (currentTextIndex < prologueTexts.Length)
        {
            if (prologueTexts[currentTextIndex] != null)
                prologueTexts[currentTextIndex].gameObject.SetActive(true); // 현재 텍스트 활성화
            if (prologueImages[currentTextIndex] != null)
                prologueImages[currentTextIndex].gameObject.SetActive(true); // 현재 이미지 활성화

            StartCoroutine(TypingRoutine());
        }
    }

    // 타이핑 효과 코루틴
    IEnumerator TypingRoutine()
    {
        if (currentTextIndex < prologueTexts.Length && prologueTexts[currentTextIndex] != null)
        {
            TextMeshProUGUI currentText = prologueTexts[currentTextIndex]; // 현재 텍스트 객체 참조
            string originalText = originalTexts[currentTextIndex]; // 현재 텍스트의 원본 내용 참조

            isTyping = true;
            isTypingComplete = false;
            int typingLength = originalText.GetTypingLength();
            for (int i = 0; i <= typingLength; i++)
            {
                currentText.text = originalText.Typing(i);
                yield return new WaitForSeconds(0.05f);
            }
            isTyping = false;
            isTypingComplete = true; // 타이핑 완료 상태로 변경
        }
    }


    private void NextText()
    {
        // 타이핑이 완료되고 타이핑 중이 아닌 경우에만 다음 텍스트로 넘어감
        if (currentTextIndex < prologueTexts.Length - 1 && isTypingComplete && !isTyping)
        {
            // 현재 텍스트와 이미지를 비활성화
            DeactivateCurrentTextAndImage();

            currentTextIndex++; // 인덱스 증가

            // 다음 텍스트와 이미지 활성화
            ActivateNextTextAndImage();

            // 다음 텍스트의 타이핑 시작
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
