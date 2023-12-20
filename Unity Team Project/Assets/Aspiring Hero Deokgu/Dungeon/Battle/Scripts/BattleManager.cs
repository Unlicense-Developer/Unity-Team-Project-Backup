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
            // 초기화 로직
        }

        void Update()
        {
            switch (state)
            {
                case BattleState.EnemyTurn:
                    // 적 턴 처리

                    break;
                case BattleState.PlayerTurn:
                    // 플레이어 턴 처리

                    break;
                    // 기타 상태 처리
            }
        }

        // 전투 시작 메소드
        private void StartBattle()
        {
            state = BattleState.EnemyTurn;
            // 전투 시작에 필요한 로직

        }

        // 몬스터와의 충돌 감지
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
                // 성공적인 대응
                SuccessfulReaction();
                // 몬스터 HP 감소 로직
            }
            else
            {
                // 실패한 대응
                FailedReaction();
                // 플레이어 HP 감소 로직
            }

            // 다음 턴으로 진행하는 로직
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
            return 10; // 예시 데미지 값
        }
    }
}
