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

        public RuntimeAnimatorController battleAnimatorController; // ������ �ִϸ����� ��Ʈ�ѷ�
        private RuntimeAnimatorController originalAnimatorController; // ���� �ִϸ����� ��Ʈ�ѷ� ����
        private CharacterController characterController;
        private ThirdPersonController thirdPersonController;

        public EnemyBattleController currentEnemyBattleController; // ���� ���� ���� ���� EnemyController
        private EnemyBattleController.EnemyAttack currentEnemyAttack;

        private int currentCombo = 0; // ���� �޺� ������ ����

        private void Awake()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            thirdPersonController = GetComponent<ThirdPersonController>();
            originalAnimatorController = animator.runtimeAnimatorController; // ���� �ִϸ����� ��Ʈ�ѷ� ����
        }

        void Start()
        {
            battleManager = FindObjectOfType<BattleManager>(); // �ٸ� GameObject�� �ִ� ��츦 ���� ����
            uiManager = FindObjectOfType<UIManager>(); // �ٸ� GameObject�� �ִ� ��츦 ���� ����
        }

        public void ReactToEnemyAttack(EnemyBattleController.EnemyAttack attack)
        {
            currentEnemyAttack = attack;
        }

        public void ProcessPlayerReaction(string playerAction)
        {
            // ���� ���°� PlayerTurn�̰�, �÷��̾ ���� �������� �ʾ��� ���� �ൿ ó��
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

                // ���� ������ ������ �޼ҵ�� ó��
                battleManager.EndTutorial();
                int attackVariant = Random.Range(1, 4); // ���� ���� ���� ���
                string attackSoundName = $"MaleAttack({attackVariant})";
                WorldSoundManager.Instance.PlaySFX(attackSoundName);
                battleManager.playerTurn = false; // �÷��̾ ������ ���������Ƿ� ���� �ൿ�� ����
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
            // �ִϸ��̼��� ������ ȣ��� �޼ҵ�
        }

        private void HandleReaction(EnemyBattleController.EnemyAttack effectiveAttack)
        {
            if (currentEnemyAttack == effectiveAttack)
                battleManager.SuccessfulReaction();
            else
                battleManager.FailedReaction();
        }

        // �Ϲ� ���ݿ� ���� ���� �ִϸ��̼� ����
        public void PlayAttack()
        {
            int comboNumber = Random.Range(1, 5); // 1���� 4������ ���� ����
            animator.SetTrigger("Attack" + comboNumber.ToString());
        }

        private void PlayComboAttack()
        {
            currentCombo++; // �޺� ���� ����
            if (currentCombo > 4) // 4�� �ʰ��ϸ� �ٽ� 1�� �ʱ�ȭ
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
                Debug.Log("PlayerController: �÷��̾� �ǰ�!");

                // ���� �ǰ� �ִϸ��̼� ����
                int hitVariant = Random.Range(1, 4); // 1���� 3���� ���� ���� ����
                animator.SetTrigger($"Hitted{hitVariant}"); // ��: "Hitted1", "Hitted2", "Hitted3"
                battleManager.paturnCount = 0;

                // �ǰ� ����Ʈ Ȱ��ȭ
                hittedEffectParticleSystem.Play();

                // ���� �ǰ� ���� ���
                string hitSoundName = $"MaleHitted({hitVariant})";
                WorldSoundManager.Instance.PlaySFX(hitSoundName);

                // �ǰ� ������ ����
                battleManager.playerStatus.ReceiveDamage(battleManager.currentEnemyStatus.AttackDamage);

                // �÷��̾� �� ���� �� ��� �ð� �� �� �� ����
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
