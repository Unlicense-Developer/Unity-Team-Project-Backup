using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;

namespace Dungeon
{
    public class EnemyBattleController : MonoBehaviour
    {
        private int consecutivePlayerHits = 0;
        private bool specialAttackPerformed = false; // Ư�� ���� ���� ���θ� ����
        public bool isBreak = false;

        private ParticleSystem hittedEffectParticleSystem;
        private ParticleSystem auraEffectParticleSystem; // ���� ����Ʈ ��ƼŬ �ý���

        public enum EnemyAttack { Smash, Swipe, Stab }
        private EnemyAttack currentAttack; // ���� ���� ���� ������ �����ϴ� ����

        private Animator animator;
        private Renderer[] childRenderers;

        private BattleManager battleManager;
        private EnemyStatus enemy;
        private PlayerStatus player;
        private PlayerBattleController playerBattleController;
        private List<string> playerRecentActions = new List<string>();

        private void Awake()
        {
            animator = GetComponent<Animator>();
            childRenderers = GetComponentsInChildren<Renderer>();
            battleManager = FindObjectOfType<BattleManager>();
            enemy = GetComponent<EnemyStatus>();

            Transform hittedEffectTransform = transform.Find("HittedEffect");
            if (hittedEffectTransform != null)
            {
                GameObject hittedEffect = hittedEffectTransform.gameObject;
                hittedEffectParticleSystem = hittedEffect.GetComponent<ParticleSystem>();
            }

            Transform auraEffectTransform = transform.Find("AuraEffect");
            if (auraEffectTransform != null)
            {
                GameObject auraEffect = auraEffectTransform.gameObject;
                auraEffectParticleSystem = auraEffect.GetComponent<ParticleSystem>();
            }
        }

        void Start()
        {
            playerBattleController = FindObjectOfType<PlayerBattleController>();
            player = FindObjectOfType<PlayerStatus>();
        }

        public void EnemyTurn()
        {
            if (battleManager.enemyTurn && battleManager.playerStatus.isDeath == false)
            {
                // ù ��° Ư�� ���� ���� �˻�: ü�� 30% ���� �Ǵ� turnCount 10 �̻�
                if (!specialAttackPerformed && (enemy.CurrentHealth <= 30 || battleManager.turnCount >= 10))
                {
                    PerformSpecialAttack();
                    specialAttackPerformed = true; // Ư�� ���� ���� ǥ��
                    Debug.Log("EnemyController: Ư�� ���� Ȱ��ȭ");
                }
                // ���� �Ͽ����� 30% Ȯ���� Ư�� ����
                // else if (specialAttackPerformed && Random.Range(0, 100) < 30)
                // {
                //     PerformSpecialAttack();
                // }
                else
                {
                    // �Ϲ� ���� ����
                    EnemyAttack attack = ChooseAttack();

                    switch (attack)
                    {
                        case EnemyAttack.Smash:
                            currentAttack = EnemyAttack.Smash;
                            PlayRandomAnimation("Smash", 2);
                            break;
                        case EnemyAttack.Swipe:
                            currentAttack = EnemyAttack.Swipe;
                            PlayRandomAnimation("Swipe", 3);
                            break;
                        case EnemyAttack.Stab:
                            currentAttack = EnemyAttack.Stab;
                            PlayRandomAnimation("Stab", 2);
                            break;
                    }

                    playerBattleController.ReactToEnemyAttack(attack);
                }

                battleManager.enemyTurn = false;
                Debug.Log("EnemyController: �� �� ����");
                battleManager.playerTurn = true;

                // �÷��̾� ������ ��ȯ
                battleManager.state = BattleManager.BattleState.PlayerTurn;
            }
        }

        private EnemyAttack ChooseAttack()
        {
            // ���������� �߰��ϱ� ���� ���� Ȯ���� ������ ������ ����
            if (Random.Range(0, 100) < 30) // ��: 30% Ȯ���� ������ ����
            {
                return (EnemyAttack)Random.Range(0, 3);
            }

            return ChooseAttackBasedOnPlayerBehavior();
        }

        private EnemyAttack ChooseAttackBasedOnPlayerBehavior()
        {
            if (playerRecentActions.Count == 0)
            {
                return DefaultStrategy();
            }

            var mostCommonAction = AnalyzePlayerActions();

            switch (mostCommonAction)
            {
                case "CounterAttack":
                    return EnemyAttack.Smash;
                case "ShieldAttack":
                    return EnemyAttack.Stab;
                case "Guard":
                    return EnemyAttack.Swipe;
                default:
                    return DefaultStrategy();
            }
        }

        private EnemyAttack DefaultStrategy()
        {
            return (EnemyAttack)Random.Range(0, 3);
        }

        // ���� ���� ���� ������ ��ȯ�ϴ� �޼ҵ�
        public EnemyAttack GetCurrentAttackType()
        {
            return currentAttack;
        }

        private void PerformSpecialAttack()
        {
            if (!gameObject.activeInHierarchy)
                return;

            // ���� ����Ʈ Ȱ��ȭ
            if (auraEffectParticleSystem != null)
            {
                auraEffectParticleSystem.Play();
            }
            else
            {
                Debug.LogError("Aura effect particle system not found!");
            }

            // ĳ���� ������ ���������� ����
            foreach (Renderer renderer in childRenderers)
            {
                renderer.material.color = new Color(1.0f, 0.5f, 0.5f, 1.0f);
            }

            // �ִϸ��̼� �ӵ� ����
            animator.speed = 1.5f; // ��: �ӵ��� 1.5��� ����

            StartCoroutine(battleManager.EndPlayerTurnRoutine());
        }

        private void PlayRandomAnimation(string attackType, int variations)
        {
            int variant = Random.Range(1, variations + 1); // 1���� variations������ ���� ����
            string triggerName = attackType + variant.ToString(); // ��: "Smash1"
            animator.SetTrigger(triggerName);
            Debug.Log("EnemyController: " + triggerName + " ����");
        }

        public void OnPlayerAttackSuccess()
        {
            consecutivePlayerHits++;
            if (consecutivePlayerHits >= 3) // �÷��̾ ���� 3�� ���� ���� ��
            {
                ActivateDefenseMode();
                consecutivePlayerHits = 0;
            }
        }

        public void AttackPlayer()
        {
            playerBattleController.PlayerHitted();
        }

        public void EnemyHitted()
        {
            // ������Ʈ�� Ȱ��ȭ�Ǿ� ���� ������ �ƹ��͵� ���� ����
            if (!gameObject.activeInHierarchy)
                return;

            if (battleManager.paturnCount == 1 || isBreak == true)
            {
                Debug.Log("EnemyController: �� �ǰ�!");

                // �ǰ� ����Ʈ Ȱ��ȭ
                hittedEffectParticleSystem.Play();

                // �ǰ� ������ ����
                battleManager.currentEnemyStatus.ReceiveDamage(battleManager.playerStatus.TotalAttackDamage);

                // �ǰ� ���� ���
                int hittedVariant = Random.Range(1, 9); // ���� ���� ���� ���
                string hittedSoundName = $"SwordSlashImpact({hittedVariant})";
                WorldSoundManager.Instance.PlaySFX(hittedSoundName);
                WorldSoundManager.Instance.PlaySFX("BoneBreak(5)");

                battleManager.paturnCount = 0;

                // ���� �ǰ� �ִϸ��̼� ����
                if (battleManager.currentEnemyStatus.isDeath == false)
                {
                    int hitVariant = Random.Range(1, 4); // 1���� 3���� ���� ���� ����
                    animator.SetTrigger($"Hitted{hitVariant}"); // ��: "Hitted1", "Hitted2", "Hitted3"

                    if (isBreak == true) animator.SetTrigger("Stunned");
                }

                // �÷��̾� �� ���� �� ��� �ð� �� �� �� ����
                if (isBreak == false) StartCoroutine(battleManager.EndPlayerTurnRoutine());
            }
        }

        public void CheckBreak()
        {
            if (battleManager.paturnCount == 1 && battleManager.currentEnemyStatus.CurrentBreakPoint <= 0)
            {
                Debug.Log("EnemyController: �� ����ȭ!");
                isBreak = true;
                animator.SetTrigger("Stunned");
                battleManager.uiManager.pressF.gameObject.SetActive(true);
                Invoke("ActivateDefenseMode", 5.0f);
            }
        }

        // ��� ��� Ȱ��ȭ ����
        private void ActivateDefenseMode()
        {
            // ������Ʈ�� Ȱ��ȭ�Ǿ� ���� ������ �ƹ��͵� ���� ����
            if (!gameObject.activeInHierarchy)
                return;

            isBreak = false;
            if (battleManager.currentEnemyStatus.isDeath == false)
            {
                animator.SetTrigger("ActivateDefense");
                battleManager.currentEnemyStatus.RestoreBreak();
                Debug.Log("EnemyController: ��� ��� Ȱ��ȭ");
            }
            battleManager.uiManager.pressF.gameObject.SetActive(false);
            StartCoroutine(battleManager.EndPlayerTurnRoutine());
        }

        private string AnalyzePlayerActions()
        {
            if (playerRecentActions.Count == 0)
                return null;

            var frequency = new Dictionary<string, int>();
            foreach (var action in playerRecentActions)
            {
                if (!frequency.ContainsKey(action))
                {
                    frequency[action] = 0;
                }
                frequency[action]++;
            }

            string mostCommonAction = null;
            int maxFrequency = 0;
            foreach (var pair in frequency)
            {
                if (pair.Value > maxFrequency)
                {
                    mostCommonAction = pair.Key;
                    maxFrequency = pair.Value;
                }
            }

            return mostCommonAction;
        }

        public void RecordPlayerAction(string action)
        {
            if (playerRecentActions.Count >= 5)
            {
                playerRecentActions.RemoveAt(0);
            }
            playerRecentActions.Add(action);
        }

        // Ʃ�丮�� ���� Ȯ��
        public void TutorialCheck()
        {
            battleManager.TutorialCheck();
        }


        // Ʃ�丮�� ���� �� �ִϸ��̼� ���߱�
        public void PauseAnimationForTutorial()
        {
            if (animator != null)
            {
                animator.speed = 0; // �ִϸ��̼� �Ͻ� ����
            }
        }

        // Ʃ�丮�� ���� �� �ִϸ��̼� �簳
        public void ResumeAnimationAfterTutorial()
        {
            if (animator != null)
            {
                animator.speed = 1; // �ִϸ��̼� �簳
            }
        }
    }
}
