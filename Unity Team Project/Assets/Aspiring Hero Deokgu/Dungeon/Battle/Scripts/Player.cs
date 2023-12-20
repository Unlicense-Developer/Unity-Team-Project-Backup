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
            Health = 100; // 초기 체력 설정
            ShieldCount = 3; // 초기 방패 횟수 설정
            uiManager = FindObjectOfType<UIManager>();
            uiManager.UpdatePlayerHealth(Health); // 초기 체력 UI 업데이트
        }

        public void ReceiveDamage(int damage)
        {
            Health -= damage;
            uiManager.UpdatePlayerHealth(Health); // 체력 변경 시 UI 업데이트
        }

        public void UseShield()
        {
            if (ShieldCount > 0)
            {
                ShieldCount--;
                // 방패 사용 로직
                // UI 업데이트 로직 필요한 경우 여기에 추가
            }
        }


        // 추가 플레이어 관련 메소드
    }
}