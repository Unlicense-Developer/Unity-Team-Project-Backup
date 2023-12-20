using UnityEngine;

namespace DungeonBattle
{
    public class EnemyController : MonoBehaviour
    {
        public enum EnemyAttack { Smash, Swipe, Stab }
        private PlayerController playerController;

        void Start()
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        public void EnemyTurn()
        {
            EnemyAttack attack = (EnemyAttack)Random.Range(0, 3);
            Debug.Log("Enemy prepares: " + attack.ToString());

            playerController.ReactToEnemyAttack(attack);
        }

        public void ReceiveAttack(string direction)
        {
            // 적이 공격 받는 로직
            Debug.Log("Enemy receives attack from direction: " + direction);
            // 피해 계산 및 반응 로직 구현
        }

        private string ChooseAttackDirection()
        {
            string[] directions = new string[] { "Up", "Down", "Left", "Right" };
            int randomIndex = Random.Range(0, directions.Length);
            return directions[randomIndex];
        }
    }
}
