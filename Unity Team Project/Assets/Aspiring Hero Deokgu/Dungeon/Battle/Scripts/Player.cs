using UnityEngine;

namespace DungeonBattle
{
    public class Player : MonoBehaviour
    {
        public int Health { get; private set; }
        public int ShieldCount { get; private set; }

        private UIManager uiManager;

        void Start()
        {
            Health = 100; // �ʱ� ü�� ����
            ShieldCount = 3; // �ʱ� ���� Ƚ�� ����
            uiManager = FindObjectOfType<UIManager>();
            uiManager.UpdatePlayerHealth(Health); // �ʱ� ü�� UI ������Ʈ
        }

        public void ReceiveDamage(int damage)
        {
            Health -= damage;
            uiManager.UpdatePlayerHealth(Health); // ü�� ���� �� UI ������Ʈ
        }

        public void UseShield()
        {
            if (ShieldCount > 0)
            {
                ShieldCount--;
                // ���� ��� ����
                // UI ������Ʈ ���� �ʿ��� ��� ���⿡ �߰�
            }
        }


        // �߰� �÷��̾� ���� �޼ҵ�
    }
}