using UnityEngine;
using UnityEngine.UI;

namespace DungeonBattle
{
    public class UIManager : MonoBehaviour
    {
        public GameObject battleUiGroup;

        public Text playerHealthText;
        public Text enemyHealthText;
        public Text shieldCountText;

        public Button shieldButton;

        private Player player;
        private Enemy enemy;

        void Start()
        {
            player = FindObjectOfType<Player>();
            enemy = FindObjectOfType<Enemy>();

            shieldButton.onClick.AddListener(OnShieldButtonClicked);
            UpdateUI();
        }

        void BattleStart()
        {
            battleUiGroup.SetActive(true);
        }

        void BattleOver()
        {
            battleUiGroup.SetActive(false);
        }

        void UpdateUI()
        {
            playerHealthText.text = "Player Health: " + player.Health;
            enemyHealthText.text = "Enemy Health: " + enemy.Health;
            shieldCountText.text = "Shields: " + player.ShieldCount;
        }

        void OnShieldButtonClicked()
        {
            player.UseShield();
            UpdateUI();
        }

        public void UpdatePlayerHealth(int health)
        {
            playerHealthText.text = "Player Health: " + health;
        }

        public void UpdateEnemyHealth(int health)
        {
            enemyHealthText.text = "Enemy Health: " + health;
        }

        // 추가 UI 업데이트 메소드
    }
}