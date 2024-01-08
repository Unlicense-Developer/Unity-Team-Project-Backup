using GrillingMeatGame;
using UnityEngine;

namespace Dungeon
{
    public class PlayerStatus : MonoBehaviour
    {
        public int Health { get; private set; }
        public int CurrentHealth { get; private set; }
        public int BaseAttackDamage { get; private set; } // �⺻ ���ݷ�
        public int AdditionalAttackDamage { get; private set; } // �߰� ���ݷ� (��� ��)
        public int TotalAttackDamage { get; private set; } // ���� ���ݷ�
        public int BaseBreakDamage { get; private set; } // �⺻ ����ȭ
        public int AdditionalBreakDamage { get; private set; } // �߰� ����ȭ (��� ��)
        public int TotalBreakDamage { get; private set; } // ���� ����ȭ
        public int PotionCount { get; private set; }
        public int Defense { get; private set; } // ���� ���� �߰�
        public bool isDeath { get; private set; }

        private UIManager uiManager;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();

            if (uiManager == null)
            {
                Debug.LogError("UIManager not found in the scene.");
                return; // UIManager�� ������ �Լ��� �����մϴ�.
            }
        }

        void Start()
        {
            Health = 100; // �ʱ� ü�� ����
            CurrentHealth = Health;
            BaseAttackDamage = 10;
            BaseBreakDamage = 10;
            Defense = 0; // ���� ����

            UpdatePotionCount(); // ���� UIManager�� null�� �ƴ϶�� Ȯ���� �� �ֽ��ϴ�.
        }

        // �� ���ݷ� ���
        public void CalculateTotalAttackDamage()
        {
            TotalAttackDamage = BaseAttackDamage + AdditionalAttackDamage;
        }

        public void CalculateTotalBreakDamage()
        {
            TotalBreakDamage = BaseBreakDamage + AdditionalBreakDamage;
        }

        public void ReceiveDamage(int damage)
        {
            if (!isDeath)
            {
                int finalDamage = Mathf.Max(damage - Defense, 0); // ���� ����
                CurrentHealth -= finalDamage;
                uiManager.UpdatePlayerHealth(CurrentHealth); // ü�� ���� �� UI ������Ʈ

                if (CurrentHealth <= 0)
                {
                    isDeath = true;
                    // ��� ��� ���
                    GetComponent<Animator>().SetTrigger("Death");
                    // ��� ���� ���
                    WorldSoundManager.Instance.PlaySFX("MaleScream");
                }
            }
        }

        public void UpdatePotionCount()
        {
            PotionCount = InventoryManager.Instance.GetItemCount("Potion");
            uiManager.UpdateUI(); // UI�� ���� ������ ������Ʈ�ϴ� �Լ� ȣ��
        }

        public void UsePotion()
        {
            if (PotionCount > 0 && !isDeath)
            {
                // �κ��丮 ���� ���� �Ҹ�
                InventoryManager.Instance.RemoveItem("Potion");
                // ���� ��� ����
                CurrentHealth = Health;
                // ���� ���
                WorldSoundManager.Instance.PlaySFX("Drink");
                // ���� ��� �� PotionCount ������Ʈ
                UpdatePotionCount();
            }
        }

        public void Revive(int health)
        {
            CurrentHealth = health;
            // �ʿ��� ��� �߰� ����...
            isDeath = false;
        }
    }
}