using UnityEngine;

namespace DungeonBattle
{
    public class BattleManager : MonoBehaviour
    {
        private UIManager uiManager;
        private Player player;
        private Enemy enemy;
        public enum BattleState { Start, EnemyTurn, PlayerTurn, Won, Lost }

        public BattleState state;

        void Start()
        {
            state = BattleState.Start;
            // �ʱ�ȭ ����
        }

        void Update()
        {
            switch (state)
            {
                case BattleState.EnemyTurn:
                    // �� �� ó��

                    break;
                case BattleState.PlayerTurn:
                    // �÷��̾� �� ó��

                    break;
                    // ��Ÿ ���� ó��
            }
        }

        // ���� ���� �޼ҵ�
        private void StartBattle()
        {
            state = BattleState.EnemyTurn;
            // ���� ���ۿ� �ʿ��� ����

        }

        // ���Ϳ��� �浹 ����
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Monster"))
            {
                StartBattle();
            }
        }

        public void ProcessPlayerReaction(EnemyController.EnemyAttack enemyAttack, string playerAction)
        {
            if ((enemyAttack == EnemyController.EnemyAttack.Smash && playerAction == "Evade") ||
                (enemyAttack == EnemyController.EnemyAttack.Swipe && playerAction == "CounterAttack") ||
                (enemyAttack == EnemyController.EnemyAttack.Stab && playerAction == "Defend"))
            {
                // �������� ����
                SuccessfulReaction();
                // ���� HP ���� ����
            }
            else
            {
                // ������ ����
                FailedReaction();
                // �÷��̾� HP ���� ����
            }

            // ���� ������ �����ϴ� ����
        }
        public void SuccessfulReaction()
        {
            enemy.ReceiveDamage(CalculateDamage());
        }

        public void FailedReaction()
        {
            player.ReceiveDamage(enemy.AttackDamage);
        }

        private int CalculateDamage()
        {
            return 10; // ���� ������ ��
        }
    }
}
