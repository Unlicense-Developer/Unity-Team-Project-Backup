using PlayerController;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeon
{
    public class PlayerBattleController : MonoBehaviour
    {
        public Animator animator;

        private BattleManager battleManager;
        private UIManager uiManager;

        public ParticleSystem hittedEffectParticleSystem;
        public ParticleSystem attackEffectParticleSystem;
        public ParticleSystem swordEffectParticleSystem;
        public ParticleSystem shieldEffectParticleSystem;

        public RuntimeAnimatorController battleAnimatorController; // 전투용 애니메이터 컨트롤러
        private RuntimeAnimatorController originalAnimatorController; // 기존 애니메이터 컨트롤러 저장
        private CharacterController characterController;
        private ThirdPersonController thirdPersonController;

        public EnemyBattleController currentEnemyBattleController; // 현재 전투 중인 적의 EnemyController
        private EnemyBattleController.EnemyAttack currentEnemyAttack;

        private int currentCombo = 0; // 현재 콤보 공격의 순서

        private void Awake()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            thirdPersonController = GetComponent<ThirdPersonController>();
            originalAnimatorController = animator.runtimeAnimatorController; // 기존 애니메이터 컨트롤러 저장
        }

        void Start()
        {
            battleManager = FindObjectOfType<BattleManager>(); // 다른 GameObject에 있는 경우를 위해 변경
            uiManager = FindObjectOfType<UIManager>(); // 다른 GameObject에 있는 경우를 위해 변경
        }

        public void ReactToEnemyAttack(EnemyBattleController.EnemyAttack attack)
        {
            currentEnemyAttack = attack;
        }

        public void ProcessPlayerReaction(string playerAction)
        {
            // 현재 상태가 PlayerTurn이고, 플레이어가 아직 공격하지 않았을 때만 행동 처리
            if (battleManager.state == BattleManager.BattleState.PlayerTurn && battleManager.playerTurn)
            {
                switch (playerAction)
                {
                    case "CounterAttack":
                        HandleReaction(EnemyBattleController.EnemyAttack.Smash);
                        animator.SetTrigger("CounterAttack");
                        break;
                    case "ShieldAttack":
                        HandleReaction(EnemyBattleController.EnemyAttack.Stab);
                        animator.SetTrigger("ShieldAttack");
                        break;
                    case "Guard":
                        HandleReaction(EnemyBattleController.EnemyAttack.Swipe);
                        animator.SetTrigger("Guard");
                        break;
                    case "ComboAttack":
                        PlayComboAttack();
                        break;
                }

                // 공통 로직을 별도의 메소드로 처리
                battleManager.EndTutorial();
                int attackVariant = Random.Range(1, 4); // 랜덤 공격 사운드 재생
                string attackSoundName = $"MaleAttack({attackVariant})";
                WorldSoundManager.Instance.PlaySFX(attackSoundName);
                battleManager.playerTurn = false; // 플레이어가 공격을 수행했으므로 다음 행동을 방지
            }
            else
            {
                Debug.Log("It's not player's turn.");
            }
        }

        public void OnCounterAttackButtonClicked()
        {
            ProcessPlayerReaction("CounterAttack");
        }

        public void OnShieldAttackButtonClicked()
        {
            ProcessPlayerReaction("ShieldAttack");
        }

        public void OnGuardButtonClicked()
        {
            ProcessPlayerReaction("Guard");
        }

        /*public void OnComboAttackButtonClicked()
        {
            ProcessPlayerReaction("ComboAttack");
        }*/

        public void OnPotionButtonClicked()
        {
            battleManager.playerStatus.UsePotion();
            uiManager.UpdateUI();
        }

        public void OnAnimationEnd()
        {
            // 애니메이션이 끝나면 호출될 메소드
        }

        private void HandleReaction(EnemyBattleController.EnemyAttack effectiveAttack)
        {
            if (currentEnemyAttack == effectiveAttack)
                battleManager.SuccessfulReaction();
            else
                battleManager.FailedReaction();
        }

        // 일반 공격에 대한 랜덤 애니메이션 실행
        public void PlayAttack()
        {
            int comboNumber = Random.Range(1, 5); // 1부터 4까지의 랜덤 숫자
            animator.SetTrigger("Attack" + comboNumber.ToString());
        }

        private void PlayComboAttack()
        {
            currentCombo++; // 콤보 순서 증가
            if (currentCombo > 4) // 4을 초과하면 다시 1로 초기화
            {
                currentCombo = 1;
            }

            animator.SetTrigger("ComboAttack" + currentCombo.ToString());
        }

        public void AttackEnemy()
        {
            currentEnemyBattleController.EnemyHitted();
            // currentEnemyBattleController.CheckBreak();
        }

        public void PlayerHitted()
        {
            if (battleManager.paturnCount == 2 || battleManager.paturnCount == 0)
            {
                Debug.Log("PlayerController: 플레이어 피격!");

                // 랜덤 피격 애니메이션 실행
                int hitVariant = Random.Range(1, 4); // 1부터 3까지 랜덤 숫자 생성
                animator.SetTrigger($"Hitted{hitVariant}"); // 예: "Hitted1", "Hitted2", "Hitted3"
                battleManager.paturnCount = 0;

                // 피격 이펙트 활성화
                hittedEffectParticleSystem.Play();

                // 랜덤 피격 사운드 재생
                string hitSoundName = $"MaleHitted({hitVariant})";
                WorldSoundManager.Instance.PlaySFX(hitSoundName);

                // 피격 데미지 적용
                battleManager.playerStatus.ReceiveDamage(battleManager.currentEnemyStatus.AttackDamage);

                // 플레이어 턴 종료 및 대기 시간 후 적 턴 시작
                battleManager.playerTurn = false;
                StartCoroutine(battleManager.EndPlayerTurnRoutine());
            }
        }

        public void SwordHitted()
        {
            if (battleManager.paturnSuccess == true)
            {
                attackEffectParticleSystem.Play();
                battleManager.paturnSuccess = false;
            }

            if (currentEnemyBattleController.isBreak == true)
            {
                currentEnemyBattleController.EnemyHitted();
            }
        }

        public void ShieldHitted()
        {
            if (battleManager.paturnSuccess == true)
            {
                attackEffectParticleSystem.Play();
                battleManager.paturnSuccess = false;
            }
        }

        public void SwitchToBattleAnimator()
        {
            currentEnemyBattleController = battleManager.currentEnemyBattleController;

            if (battleAnimatorController != null)
            {
                characterController.enabled = false;
                thirdPersonController.enabled = false;
                animator.runtimeAnimatorController = battleAnimatorController;
            }
        }

        public void SwitchToOriginalAnimator()
        {
            characterController.enabled = true;
            thirdPersonController.enabled = true;
            animator.runtimeAnimatorController = originalAnimatorController;
        }

        void Update()
        {
            if (battleManager.currentEnemyBattleController != null)
            {
                if (Input.GetKeyDown(KeyCode.A)) OnCounterAttackButtonClicked();
                if (Input.GetKeyDown(KeyCode.S)) OnShieldAttackButtonClicked();
                if (Input.GetKeyDown(KeyCode.D)) OnGuardButtonClicked();
                if (Input.GetKeyDown(KeyCode.H)) OnPotionButtonClicked();
                if (Input.GetKeyDown(KeyCode.F) && battleManager.currentEnemyBattleController.isBreak)
                    PlayComboAttack();
            }
        }
    }
}
