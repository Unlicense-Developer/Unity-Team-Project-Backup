using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cinemachine;

namespace Dungeon
{
    public class BattleManager : MonoBehaviour
    {
        public UIManager uiManager;
        private GameObject player;
        private GameObject currentEnemy;

        public GameObject weapon;
        public GameObject shield;

        public PlayerStatus playerStatus; // 현재 전투 중인 플레이어
        public PlayerBattleController playerBattleController;
        public EnemyStatus currentEnemyStatus; // 현재 전투 중인 적
        public EnemyBattleController currentEnemyBattleController; // 현재 전투 중인 적의 EnemyController

        public CinemachineVirtualCamera battleCamera; // 전투용 가상 카메라 참조

        public enum BattleState { Start, EnemyTurn, PlayerTurn, Won, Lost }

        public BattleState state;
        public int turnCount = 0; // 턴 카운트
        public int paturnCount = 0; // 0 == 기본값, 1 == 성공, 2 == 실패
        public bool paturnSuccess = false;
        public float delayTime;

        private bool turnStarted = false; // 턴 시작 여부를 체크하는 변수
        public bool playerTurn = false;
        public bool enemyTurn = false;

        // 튜토리얼용 첫 전투 여부 체크
        private bool isFirstBattle = true;
        private int tutorialCount = 0;

        // 초기 상태를 저장하기 위한 변수 선언
        private int initialPlayerHealth;
        private int initialEnemyHealth;
        private int initialEnemyBreakPoint;


        private void Awake()
        {
            uiManager = GetComponent<UIManager>();
            playerStatus = FindObjectOfType<PlayerStatus>();
            playerBattleController = FindObjectOfType<PlayerBattleController>();
        }

        void Start()
        {
            state = BattleState.Start;
            // 초기화 로직...
        }

        void Update()
        {
            switch (state)
            {
                case BattleState.EnemyTurn:
                    // 전투 상태 확인
                    CheckEndBattle();
                    // 적 턴 처리 시작
                    enemyTurn = true;
                    currentEnemyBattleController.EnemyTurn();
                    break;

                case BattleState.PlayerTurn:
                    // 플레이어 턴 처리 시작
                    //playerController.playerTurn();
                    break;

                    // ... 기타 상태들의 처리
            }
        }

        // 전투 시작 메소드
        public void SetUpBattle(GameObject currentEnemy)
        {
            currentEnemyStatus = currentEnemy.GetComponent<EnemyStatus>();
            currentEnemyBattleController = currentEnemy.GetComponent<EnemyBattleController>();

            if (currentEnemyBattleController == null)
            {
                Debug.LogError("EnemyController not found on the current enemy");
                return;
            }

            // 적 조우 UI 애니메이션 재생
            uiManager.BattleStartEffect();

            // 전투 BGM 활성화
            WorldSoundManager.Instance.StartBattleMode();

            // 적 태그 변경
            currentEnemy.tag = "EnemyInBattle";

            // 플레이어와 적을 마주보게 위치 조정
            OrientCharactersForBattle();

            // 카메라 전환 로직 (전투용 카메라로 전환)
            SwitchToBattleCamera();

            // 플레이어 준비 시간 제공 (예: 몇 초간의 카운트다운)
            StartCoroutine(ProvidePreparationTime(currentEnemy));

            // 초기 상태 저장
            initialPlayerHealth = playerStatus.CurrentHealth;
            initialEnemyHealth = currentEnemyStatus.CurrentHealth;
            initialEnemyBreakPoint = currentEnemyStatus.CurrentBreakPoint;
        }

        private void OrientCharactersForBattle()
        {
            // "Player" 태그를 가진 객체를 찾아 player 변수에 할당
            player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player object not found");
                return;
            }

            // "Enemy" 태그를 가진 객체를 찾아 currentEnemy 변수에 할당
            currentEnemy = GameObject.FindWithTag("EnemyInBattle");
            if (currentEnemy == null)
            {
                Debug.LogError("Enemy object not found");
                return;
            }

            // 플레이어 애니메이터 전투용으로 전환
            if (playerBattleController != null)
            {
                playerBattleController.SwitchToBattleAnimator();
                playerBattleController.animator.SetTrigger("Idle");
            }

            // 플레이어와 적이 서로를 바라보게 설정
            player.transform.LookAt(currentEnemy.transform.position);
            currentEnemy.transform.LookAt(player.transform.position);

            // 플레이어와 적 사이의 거리를 설정
            float desiredDistance = 3.0f; // 원하는 거리 설정
            Vector3 directionToEnemy = (currentEnemy.transform.position - player.transform.position).normalized;

            // 플레이어의 위치를 적에게서 원하는 거리만큼 떨어지게 조정
            player.transform.position = currentEnemy.transform.position - directionToEnemy * desiredDistance;
            // 적의 위치도 플레이어에게서 원하는 거리만큼 떨어지게 조정
            currentEnemy.transform.position = player.transform.position + directionToEnemy * desiredDistance;

            // 방향 다시 조정
            player.transform.LookAt(currentEnemy.transform.position);
            currentEnemy.transform.LookAt(player.transform.position);

            // 플레이어 장비 활성화
            weapon.SetActive(true);
            shield.SetActive(true);
            playerStatus.CalculateTotalAttackDamage();
            playerStatus.CalculateTotalBreakDamage();
        }

        private void SwitchToBattleCamera()
        {
            // 가상 카메라의 Follow와 Look At 설정
            if (battleCamera != null)
            {
                battleCamera.Follow = player.transform;
                battleCamera.LookAt = currentEnemy.transform;

                // 가상 카메라 활성화
                battleCamera.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Battle camera not assigned");
            }
        }

        private IEnumerator ProvidePreparationTime(GameObject currentEnemy)
        {
            // 준비 시간 카운트다운
            yield return new WaitForSeconds(4); // 예시: 2초간 대기

            // 전투 UI 활성화
            uiManager.BattleStart();

            // 본격적인 전투 시작
            StartBattleWithEnemy(currentEnemy);
        }

        public void StartBattleWithEnemy(GameObject currentEnemy)
        {
            if (currentEnemy != null)
            {
                state = BattleState.EnemyTurn;
                Debug.Log("BattleManager: 전투 시작");
            }
        }

        public void ProcessPlayerReaction(EnemyBattleController.EnemyAttack enemyAttack, string playerAction)
        {
            if ((enemyAttack == EnemyBattleController.EnemyAttack.Smash && playerAction == "CounterAttack") ||
                (enemyAttack == EnemyBattleController.EnemyAttack.Swipe && playerAction == "ShieldAttack") ||
                (enemyAttack == EnemyBattleController.EnemyAttack.Stab && playerAction == "Guard"))
            {
                // 성공적인 대응
                SuccessfulReaction();
            }
            else
            {
                // 실패한 대응
                FailedReaction();
            }
        }

        public void SuccessfulReaction()
        {
            paturnCount = 1;
            paturnSuccess = true;
            Debug.Log("BattleManager: 패턴 판정 성공");
            // 랜덤 패링 사운드 선택 및 재생
            int parryVariant = Random.Range(1, 4); // 1부터 3까지 랜덤 숫자 생성
            string parrySoundName = $"Parrying({parryVariant})";
            WorldSoundManager.Instance.PlaySFX(parrySoundName);
            // 무력화 체크
            currentEnemyStatus.ReceiveBreak(playerStatus.TotalBreakDamage);
            currentEnemyBattleController.CheckBreak();
        }

        public void FailedReaction()
        {
            paturnCount = 2;
            Debug.Log("BattleManager: 패턴 판정 실패");
        }

        public IEnumerator EndPlayerTurnRoutine()
        {
            Debug.Log("BattleManager: 플레이어 턴 종료 대기");
            float delayTime = Random.Range(2.0f, 3.0f);
            yield return new WaitForSeconds(delayTime);

            // 플레이어의 턴이 끝날 때 호출되는 메소드
            turnCount++; // 플레이어 턴 카운트 증가

            // 다음 적 턴으로 상태 변경
            state = BattleState.EnemyTurn;
            Debug.Log("BattleManager: 다음 턴 시작");
        }

        // 전투 상태 확인 및 종료
        public void CheckEndBattle()
        {
            if (playerStatus.CurrentHealth <= 0 || (currentEnemyStatus != null && currentEnemyStatus.CurrentHealth <= 0))
            {
                EndBattle();
            }
        }

        // 전투 종료 처리
        private void EndBattle()
        {
            if (playerStatus.CurrentHealth <= 0)
            {
                state = BattleState.Lost;
                WorldSoundManager.Instance.PlaySFX("GameOver");
            }
            else
            {
                state = BattleState.Won;
                WorldSoundManager.Instance.PlaySFX("Victory");
            }

            // 배틀 UI 비활성화
            uiManager.BattleOver();
            // 배틀 BGM 비활성화
            WorldSoundManager.Instance.EndBattleMode();
            // 가상 카메라 비활성화
            Invoke("CameraReset", 2.0f);

            if (playerStatus.isDeath == false)
            {
                // 전투 초기화 로직
                Invoke("ResetForNextBattle", 0.1f);

                if (currentEnemy != null)
                {
                    currentEnemy.gameObject.tag = "Enemy"; // 태그 변경
                    currentEnemy.gameObject.SetActive(false); // 적 비활성화
                }

                // 플레이어 애니메이터 비전투화
                if (playerBattleController != null)
                {
                    playerBattleController.SwitchToOriginalAnimator();
                }
            }
        }

        // 다음 전투를 위한 초기화 메소드
        private void ResetForNextBattle()
        {
            // 상태, 턴 카운트, 패턴 카운트 등을 초기화합니다.
            state = BattleState.Start;
            turnCount = 0;
            paturnCount = 0;
            playerTurn = false;
            enemyTurn = false;

            // 플레이어와 적의 상태를 초기화합니다. (예: 체력, 무력화 상태 등)
            // 플레이어 장비 비활성화
            weapon.SetActive(false);
            shield.SetActive(false);
            // playerStatus.ResetStatusForNewBattle();
            // 다음 전투를 위한 적 설정 또는 생성 로직이 필요한 경우 여기에 추가합니다.

            Debug.Log("전투 시스템 초기화 완료. 다음 전투 준비 완료.");
        }
        public void RetryBattle()
        {
            // 플레이어와 적 상태를 초기 상태로 복원
            playerStatus.Revive(initialPlayerHealth);
            currentEnemyStatus.SetHealth(initialEnemyHealth);
            currentEnemyStatus.SetBreakPoint(initialEnemyBreakPoint);

            // 전투 상태 리셋
            ResetBattleState();

            // 전투 UI 업데이트
            uiManager.UpdateUI();

            SetUpBattle(currentEnemy);
        }

        private void ResetBattleState()
        {
            state = BattleState.Start;
            turnCount = 0;
            paturnCount = 0;
            playerTurn = false;
            enemyTurn = false;

            // 필요한 경우 추가 로직...
        }

        void CameraReset()
        {
            battleCamera.gameObject.SetActive(false);
        }

        public void TutorialCheck()
        {
            tutorialCount++;

            if (tutorialCount < 3)
            {
                // 현재 적의 공격 유형을 가정한 예시 변수입니다.
                // 실제로는 현재 적의 공격 유형을 참조해야 합니다.
                EnemyBattleController.EnemyAttack currentEnemyAttackType = currentEnemyBattleController.GetCurrentAttackType();

                StartTutorial();  // 게임 일시 정지
                HighlightButton(currentEnemyAttackType);  // 해당 버튼 강조
            }
        }

        // 튜토리얼 시작
        public void StartTutorial()
        {
            currentEnemyBattleController.PauseAnimationForTutorial();
            uiManager.tutorial.gameObject.SetActive(true);
            // 튜토리얼 관련 UI 로직
        }

        // 튜토리얼 종료
        public void EndTutorial()
        {
            currentEnemyBattleController.ResumeAnimationAfterTutorial();
            uiManager.tutorial.gameObject.SetActive(false);
            // 게임 로직 재개
            uiManager.StopHighlight(uiManager.currentButton);
        }

        private void HighlightButton(EnemyBattleController.EnemyAttack attackType)
        {
            switch (attackType)
            {
                case EnemyBattleController.EnemyAttack.Smash:
                    // 카운터 어택 버튼 강조
                    uiManager.HighlightButton(uiManager.counterAttackButton);
                    Debug.Log("카운터 어택 버튼 강조");
                    break;
                case EnemyBattleController.EnemyAttack.Stab:
                    // 쉴드 어택 버튼 강조
                    uiManager.HighlightButton(uiManager.shieldAttackButton);
                    Debug.Log("쉴드 어택 버튼 강조");
                    break;
                case EnemyBattleController.EnemyAttack.Swipe:
                    // 가드 버튼 강조
                    uiManager.HighlightButton(uiManager.guardButton);
                    Debug.Log("가드 버튼 강조");
                    break;
            }
        }

    }
}
