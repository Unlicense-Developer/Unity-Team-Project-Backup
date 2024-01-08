using UnityEngine;

namespace Dungeon
{
    public class EnemyStatus : MonoBehaviour
    {
        public int Health;
        public int CurrentHealth;
        public int BreakPoint;
        public int CurrentBreakPoint { get; private set; }
        public int AttackDamage;
        public bool isDeath { get; private set; }

        private UIManager uiManager;

        void Start()
        {
            CurrentHealth = Health;

            CurrentBreakPoint = BreakPoint;

            uiManager = FindObjectOfType<UIManager>();
        }

        public void ReceiveDamage(int damage)
        {
            if (!isDeath)
            {
                CurrentHealth -= damage;
                uiManager.UpdateEnemyHealth(CurrentHealth); // ü�� ���� �� UI ������Ʈ

                if (CurrentHealth <= 0)
                {
                    isDeath = true;
                    // ��� ��� ���
                    GetComponent<Animator>().SetTrigger("Death");
                }
            }
        }

        public void ReceiveBreak(int damage)
        {
            CurrentBreakPoint -= damage;
            uiManager.UpdateEnemyBreakPoint(CurrentBreakPoint); // ü�� ���� �� UI ������Ʈ
        }
        public void RestoreBreak()
        {
            CurrentBreakPoint = BreakPoint;
            uiManager.UpdateEnemyBreakPoint(CurrentBreakPoint); // ü�� ���� �� UI ������Ʈ
        }

        public void SetHealth(int health)
        {
            CurrentHealth = health;
            // �ʿ��� ��� �߰� ����...
        }

        public void SetBreakPoint(int breakPoint)
        {
            CurrentBreakPoint = breakPoint;
            // �ʿ��� ��� �߰� ����...
        }
    }
}