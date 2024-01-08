using UnityEngine;

namespace Dungeon
{
    public class DamageScript : MonoBehaviour
    {
        public PlayerStatus player;
        public EnemyStatus enemy;

        private void Awake()
        {
            // Player와 Enemy 참조 사전 설정
            player = FindObjectOfType<PlayerStatus>();
            enemy = FindObjectOfType<EnemyStatus>();
        }

        void Start()
        {

        }

        public void ApplyDamageToEnemy(int damageAmount)
        {
            // 플레이어의 공격력을 기반으로 적에게 데미지 적용
            enemy.ReceiveDamage(player.TotalAttackDamage);
        }

        public void ApplyDamageToPlayer(int damageAmount)
        {
            // 몬스터의 공격이 플레이어에게 적용될 때 사용될 데미지 값
            player.ReceiveDamage(damageAmount);
        }
    }
}
