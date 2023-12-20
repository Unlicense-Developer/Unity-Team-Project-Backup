using UnityEngine;
using UnityEngine.UI;

namespace DungeonBattle
{
    public class PlayerController : MonoBehaviour
    {
        private BattleManager battleManager;
        private EnemyController enemyController;

        public Button evadeButton;
        public Button counterAttackButton;
        public Button defendButton;

        private EnemyController.EnemyAttack currentEnemyAttack;
        private Player player;
        private Enemy enemy;

        void Start()
        {
            player = FindObjectOfType<Player>();
            enemy = FindObjectOfType<Enemy>();

            // 버튼 리스너 설정
            evadeButton.onClick.AddListener(() => ProcessPlayerReaction("Evade"));
            counterAttackButton.onClick.AddListener(() => ProcessPlayerReaction("CounterAttack"));
            defendButton.onClick.AddListener(() => ProcessPlayerReaction("Defend"));
        }

        public void ReactToEnemyAttack(EnemyController.EnemyAttack attack)
        {
            currentEnemyAttack = attack;
        }

        public void ProcessPlayerReaction(string playerAction)
        {
            switch (playerAction)
            {
                case "Evade":
                    Evade();
                    break;
                case "CounterAttack":
                    CounterAttack();
                    break;
                case "Defend":
                    Defend();
                    break;
            }
        }

        private void Evade()
        {
            if (currentEnemyAttack == EnemyController.EnemyAttack.Smash)
                battleManager.SuccessfulReaction();
            else
                battleManager.FailedReaction();
        }

        private void CounterAttack()
        {
            if (currentEnemyAttack == EnemyController.EnemyAttack.Swipe)
                battleManager.SuccessfulReaction();
            else
                battleManager.FailedReaction();
        }

        private void Defend()
        {
            if (currentEnemyAttack == EnemyController.EnemyAttack.Stab)
                battleManager.SuccessfulReaction();
            else
                battleManager.FailedReaction();
        }
    }
}
