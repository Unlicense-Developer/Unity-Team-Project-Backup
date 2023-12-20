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
            // ���� ���� �޴� ����
            Debug.Log("Enemy receives attack from direction: " + direction);
            // ���� ��� �� ���� ���� ����
        }

        private string ChooseAttackDirection()
        {
            string[] directions = new string[] { "Up", "Down", "Left", "Right" };
            int randomIndex = Random.Range(0, directions.Length);
            return directions[randomIndex];
        }
    }
}
