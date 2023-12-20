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
            Health = 100; // 초기 체력 설정
            uiManager = FindObjectOfType<UIManager>();
            uiManager.UpdateEnemyHealth(Health); // 초기 체력 UI 업데이트
        }

        public void ReceiveDamage(int damage)
        {
            Health -= damage;
            uiManager.UpdateEnemyHealth(Health); // 체력 변경 시 UI 업데이트
        }


        // 추가 적 관련 메소드
    }
}