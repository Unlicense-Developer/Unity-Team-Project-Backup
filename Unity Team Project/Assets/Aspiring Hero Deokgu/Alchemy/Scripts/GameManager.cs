using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Alchemy
{
    public class GameManager : MonoBehaviour
    {
        [Header("----------[System]")]
        public int frameRate;

        [Header("----------[Core]")]
        public int score; // 현재 게임 점수
        public int maxLevel; // 보석의 최대 레벨
        public bool isOver; // 게임이 끝났는지 여부

        [Header("----------[Object Pooling]")]
        public GameObject gemPrefab; // 보석 오브젝트의 프리팹
        public Transform gemGroup; // 보석 오브젝트를 담을 부모 트랜스폼
        public List<Gem> gemPool; // 오브젝트 풀링을 위한 보석 리스트

        [Range(1, 30)]
        public int poolSize; // 오브젝트 풀의 크기
        public int poolCursor; // 현재 사용할 오브젝트 풀의 인덱스
        public Gem lastGem; // 마지막으로 생성된 보석

        [Header("----------[Effect]")]
        public GameObject[] particlePrefabs; // 파티클 프리팹 배열

        public GameObject effectPrefab; // 이펙트의 프리팹
        public Transform effectGroup; // 이펙트를 담을 부모 트랜스폼
        public List<ParticleSystem> effectPool; // 오브젝트 풀링을 위한 이펙트 리스트

        [Header("----------[Audio]")]
        public AudioSource bgmPlayer; // 배경음악을 재생할 오디오 소스
        public AudioSource[] sfxPlayer; // 사운드 효과를 재생할 오디오 소스 배열
        public AudioClip[] sfxClip; // 사용할 사운드 효과 클립
        public enum Sfx { LevelUp, Next, Attach, Button, Over } // 사운드 효과 유형
        int sfxCursor; // 현재 재생할 사운드 효과의 인덱스

        [Header("----------[UI]")]
        public GameObject ui_StartGroup; // 게임 시작 화면 UI 그룹
        public GameObject ui_EndGroup; // 게임 종료 화면 UI 그룹
        public GameObject ui_GemGroup;
        public GameObject ui_BgmOn;
        public GameObject ui_BgmOff;
        public Text scoreText; // 점수를 표시할 텍스트
        public Text maxScoreText; // 최고 점수를 표시할 텍스트
        public Text subScoreText; // 게임 종료 시 점수를 표시할 텍스트

        [Header("----------[ETC]")]
        public GameObject line; // 게임 오버 라인 오브젝트
        public GameObject bottom; // 게임 화면 하단 오브젝트
        public GameObject turntable; // 원판 오브젝트


        private void Awake()
        {
            Application.targetFrameRate = frameRate; // 프레임 레이트 설정

            gemPool = new List<Gem>(); // 보석 풀 초기화
            effectPool = new List<ParticleSystem>(); // 이펙트 풀 초기화

            for (int index = 0; index < poolSize; index++) // 풀 사이즈만큼 오브젝트 생성
            {
                MakeGem();
            }

            if (!PlayerPrefs.HasKey("MaxScore")) // 최고 점수가 저장되어 있지 않으면 초기화
            {
                PlayerPrefs.SetInt("MaxScore", 0);
            }

            maxScoreText.text = PlayerPrefs.GetInt("MaxScore").ToString(); // 최고 점수 표시
        }

        private void Start()
        {
            bgmPlayer.Play(); // 배경음악 재생
        }

        // 게임 시작 함수
        public void GameStart()
        {
            // 오브젝트 활성화
            // line.SetActive(true);
            // bottom.SetActive(true);
            scoreText.gameObject.SetActive(true);
            maxScoreText.gameObject.SetActive(true);
            ui_GemGroup.SetActive(true);
            ui_StartGroup.SetActive(false);
            SfxPlay(Sfx.Button); // 버튼 클릭 사운드 효과 재생
            Invoke("NextGem", 1.5f); // 1.5초 후에 다음 보석 생성
        }

        // 보석 생성 함수
        Gem MakeGem()
        {   // 이펙트 생성
            GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);
            instantEffectObj.name = "Effect " + effectPool.Count;
            ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();
            effectPool.Add(instantEffect);

            // 보석 생성
            GameObject instantGemObj = Instantiate(gemPrefab, gemGroup);
            instantGemObj.name = "Gem " + gemPool.Count;
            Gem instantGem = instantGemObj.GetComponent<Gem>();
            instantGem.manager = this;
            instantGem.effect = instantEffect;
            gemPool.Add(instantGem);

            return instantGem;

        }

        // 풀에서 보석 가져오기 함수
        Gem GetGem()
        {
            for (int index = 0; index < gemPool.Count; index++)
            {
                poolCursor = (poolCursor + 1) % gemPool.Count;

                if (!gemPool[poolCursor].gameObject.activeSelf)
                {
                    return gemPool[poolCursor];
                }
            }

            return MakeGem();
        }


        private void NextGem()
        {
            if (isOver) // 게임 오버 상태면 종료
            {
                return;
            }

            lastGem = GetGem(); // 풀에서 보석 가져오기
            lastGem.level = Random.Range(0, maxLevel); // 레벨 무작위 설정
            lastGem.gameObject.SetActive(true); // 보석 활성화

            SfxPlay(Sfx.Next); // 다음 보석 사운드 효과 재생
            StartCoroutine(WaitNext()); // 다음 보석을 위한 대기 코루틴 시작
        }

        // 다음 보석 생성을 위한 대기 코루틴
        IEnumerator WaitNext()
        {
            while (lastGem != null) // 마지막 보석이 활성화 상태인 동안 대기
            {
                yield return null;
            }

            yield return new WaitForSeconds(2.0f); // 2초 대기

            NextGem(); // 다음 보석 생성
        }

        // 터치 다운 이벤트 처리 함수
        public void TouchDown()
        {
            if (lastGem == null) // 마지막 보석이 없으면 종료
                return;

            lastGem.Drag(); // 보석 드래그 시작
        }

        // 터치 업 이벤트 처리 함수
        public void TouchUp()
        {
            if (lastGem == null) // 마지막 보석이 없으면 종료
                return;

            lastGem.Drop(); // 보석 드랍
            lastGem = null; // 마지막 보석 참조 제거
        }

        // 게임 오버 처리 함수
        public void GameOver()
        {
            if (isOver) // 이미 게임 오버 상태면 종료
            {
                return;
            }

            isOver = true; // 게임 오버 상태로 설정

            StartCoroutine(GameOverRoutine()); // 게임 오버 처리 코루틴 시작
        }

        // 게임 오버 처리 코루틴
        IEnumerator GameOverRoutine()
        {
            // 1. 장면 안에 활성화 되어있는 모든 보석 가져오기
            Gem[] dongles = FindObjectsOfType<Gem>();

            // 2. 지우기 전에 모든 보석의 물리효과 비활성화
            for (int index = 0; index < dongles.Length; index++)
            {
                dongles[index].rigid.simulated = false;
            }

            // 3. 1번의 목록을 하나씩 접근해서 지우기
            for (int index = 0; index < dongles.Length; index++)
            {
                dongles[index].Hide(Vector3.up * 100);
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.5f);

            // 최고 점수 갱신
            int maxScore = Mathf.Max(score, PlayerPrefs.GetInt("MaxScore"));
            PlayerPrefs.SetInt("MaxScore", maxScore); // 현재 점수와 기존 최고 점수 비교 후 저장

            // 게임오버 UI 표시
            subScoreText.text = "점수: " + scoreText.text; // 게임 오버 시 점수 표시
            ui_EndGroup.SetActive(true); // 게임 오버 UI 활성화

            // bgmPlayer.Stop(); // 배경음악 정지
            SfxPlay(Sfx.Over); // 게임 오버 사운드 효과 재생
        }

        // 게임 재시작 함수
        public void Reset()
        {
            SfxPlay(Sfx.Button); // 버튼 클릭 사운드 효과 재생
            StartCoroutine(ResetCoroutine()); // 재시작 코루틴 시작
        }

        // 게임 재시작 코루틴
        IEnumerator ResetCoroutine()
        {
            yield return new WaitForSeconds(1.0f); // 1초 대기
            SceneManager.LoadScene("Alchemy"); // 메인 씬 재시작
        }

        public void ExitGame()
        {
            SceneManager.LoadScene("WorldMap"); // 메인 씬 재시작
        }

        // 사운드 효과 재생 함수
        public void SfxPlay(Sfx type)
        {
            switch (type)
            {
                case Sfx.LevelUp:
                    sfxPlayer[sfxCursor].clip = sfxClip[Random.Range(0, 3)];
                    break;
                case Sfx.Next:
                    sfxPlayer[sfxCursor].clip = sfxClip[3];
                    break;
                case Sfx.Attach:
                    sfxPlayer[sfxCursor].clip = sfxClip[4];
                    break;
                case Sfx.Button:
                    sfxPlayer[sfxCursor].clip = sfxClip[5];
                    break;
                case Sfx.Over:
                    sfxPlayer[sfxCursor].clip = sfxClip[6];
                    break;
            }

            sfxPlayer[sfxCursor].Play(); // 선택된 사운드 효과 재생
            sfxCursor = (sfxCursor + 1) % sfxPlayer.Length; // 다음 사운드 효과를 위한 인덱스 업데이트
        }

        public void ToggleBGM()
        {
            if (bgmPlayer.isPlaying)
            {
                bgmPlayer.Pause(); // BGM 정지
                ui_BgmOn.gameObject.SetActive(false);
                ui_BgmOff.gameObject.SetActive(true);
            }
            else
            {
                bgmPlayer.Play(); // BGM 재생
                ui_BgmOn.gameObject.SetActive(true);
                ui_BgmOff.gameObject.SetActive(false);
            }
        }

        // 게임 도중 업데이트 함수
        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                Application.Quit(); // "Cancel" 버튼(일반적으로 ESC)이 눌리면 게임 종료
            }
        }

        // 프레임이 끝날 때마다 호출되는 함수
        private void LateUpdate()
        {
            scoreText.text = score.ToString(); // 점수 업데이트
        }
    }
}
