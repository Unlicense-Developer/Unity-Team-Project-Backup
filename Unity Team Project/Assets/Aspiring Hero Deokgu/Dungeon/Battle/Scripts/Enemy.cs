using UnityEngine;

namespace DungeonBattle
{
    public class Enemy : MonoBehaviour
    {
        public int Health { get; private set; }
        public int AttackDamage { get; private set; }

        private UIManager uiManager;

        void Start()
        {
            Health = 100; // �ʱ� ü�� ����
            uiManager = FindObjectOfType<UIManager>();
            uiManager.UpdateEnemyHealth(Health); // �ʱ� ü�� UI ������Ʈ
        }

        public void ReceiveDamage(int damage)
        {
            Health -= damage;
            uiManager.UpdateEnemyHealth(Health); // ü�� ���� �� UI ������Ʈ
        }


        // �߰� �� ���� �޼ҵ�
    }
}