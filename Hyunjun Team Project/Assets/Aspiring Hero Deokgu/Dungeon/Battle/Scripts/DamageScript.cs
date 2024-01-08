using UnityEngine;

namespace Dungeon
{
    public class DamageScript : MonoBehaviour
    {
        public PlayerStatus player;
        public EnemyStatus enemy;

        private void Awake()
        {
            // Player�� Enemy ���� ���� ����
            player = FindObjectOfType<PlayerStatus>();
            enemy = FindObjectOfType<EnemyStatus>();
        }

        void Start()
        {

        }

        public void ApplyDamageToEnemy(int damageAmount)
        {
            // �÷��̾��� ���ݷ��� ������� ������ ������ ����
            enemy.ReceiveDamage(player.TotalAttackDamage);
        }

        public void ApplyDamageToPlayer(int damageAmount)
        {
            // ������ ������ �÷��̾�� ����� �� ���� ������ ��
            player.ReceiveDamage(damageAmount);
        }
    }
}
