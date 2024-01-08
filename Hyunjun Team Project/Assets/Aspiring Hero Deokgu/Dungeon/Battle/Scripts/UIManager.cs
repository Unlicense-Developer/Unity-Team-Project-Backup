using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


namespace Dungeon
{
    public class UIManager : MonoBehaviour
    {
        public GameObject battleUiGroup;
        public GameObject battleStartGroup;
        public GameObject battleWonGroup; // �¸� UI
        public GameObject battleLostGroup; // �й� UI

        public Image tutorial;
        public Image pressF;
        public float fadeDuration = 3.0f; // ���̵� ���� �ð� (��)
        private float currentAlpha = 1.0f; // ���� ���� �� (1.0�� ���� ������, 0.0�� ���� ����)
        private bool isFadeIn = false; // ���̵� �� ����
        private bool isFading = false; // ���̵� ������ ����
        private Color originalColor; // �ʱ� ������ �����ϱ� ���� ����

        // ��ư ���̶���Ʈ �ִϸ��̼��� ���� ����
        private float highlightScale = 1.2f; // ���̶���Ʈ �� ũ�� ����
        private float highlightDuration = 0.5f; // ���̶���Ʈ ���� �ð�
        public Button currentButton;

        public TextMeshProUGUI playerHealthText;
        public TextMeshProUGUI enemyHealthText;
        public TextMeshProUGUI enemyBreakPointText;
        public TextMeshProUGUI potionCountText;

        public Slider playerHealthSlider;
        public Slider enemyHealthSlider;
        public Slider enemyBreakPointSlider;

        public Button counterAttackButton;
        public Button shieldAttackButton;
        public Button guardButton;
        public Button potionButton;
        public Button retryButton; // �絵�� ��ư

        private BattleManager battleManager;
        private PlayerStatus playerStatus;

        private void Awake()
        {
            battleManager = FindObjectOfType<BattleManager>();
            playerStatus = FindObjectOfType<PlayerStatus>();
        }

        void Start()
        {

        }

        void Update()
        {
            IsFading();
        }

        // ���� ���� ȿ��
        public void BattleStartEffect()
        {
            Debug.Log("UIManager: battleStartImage �̹��� Ȱ��ȭ");
            StartFadeEffect(battleStartGroup, true, 1.0f);
            Invoke("FadeOutBattleStartGroup", 2.0f);
            UpdateUI();
        }
        private void FadeOutBattleStartGroup()
        {
            StartFadeEffect(battleStartGroup, false, 1.0f);
        }

        public void BattleStart()
        {
            battleUiGroup.SetActive(true);
        }

        public void BattleOver()
        {
            battleUiGroup.SetActive(false);

            if (battleManager.state == BattleManager.BattleState.Won)
            {
                // ���� �¸� UI
                StartFadeEffect(battleWonGroup, true, 1.0f);
                Invoke("FadeOutBattleWonGroup", 2.0f);
            }
            else if (battleManager.state == BattleManager.BattleState.Lost)
            {
                // ���� �й� UI
                StartFadeEffect(battleLostGroup, true, 1.0f);
            }
        }

        private void FadeOutBattleWonGroup()
        {
            StartFadeEffect(battleWonGroup, false, 1.0f);
        }

        public void FadeOutBattleLostGroup()
        {
            StartFadeEffect(battleLostGroup, false, 1.0f);
        }

        public void UpdateUI()
        {
            // null üũ �߰�
            if (playerStatus == null || battleManager == null || battleManager.currentEnemyStatus == null)
            {
                Debug.LogError("One or more references are null in UIManager.UpdateUI");
                return; // �ʿ��� ���� �� �ϳ��� null�̸� �Լ��� �����մϴ�.
            }

            playerHealthText.text = "Player Health: " + playerStatus.CurrentHealth;
            enemyHealthText.text = "Enemy Health: " + battleManager.currentEnemyStatus.CurrentHealth;
            enemyBreakPointText.text = "Break Point: " + battleManager.currentEnemyStatus.BreakPoint;
            potionCountText.text = "" + playerStatus.PotionCount;

            // �ִ� ü�� ��� ���� ü���� ���� ���
            float playerHealthRatio = (float)playerStatus.CurrentHealth / playerStatus.Health;
            float enemyHealthRatio = (float)battleManager.currentEnemyStatus.CurrentHealth / battleManager.currentEnemyStatus.Health;
            float enemyBreakPointRatio = (float)battleManager.currentEnemyStatus.CurrentBreakPoint / battleManager.currentEnemyStatus.BreakPoint;

            // Slider �� ������Ʈ
            playerHealthSlider.value = playerHealthRatio;
            enemyHealthSlider.value = enemyHealthRatio;
            enemyBreakPointSlider.value = enemyBreakPointRatio;
        }

        public void UpdatePlayerHealth(int health)
        {
            if (health <= 0) health = 0;
            playerHealthText.text = "Player Health: " + health;
            float playerHealthRatio = (float)health / playerStatus.Health;
            playerHealthSlider.value = playerHealthRatio;
        }

        public void UpdateEnemyHealth(int health)
        {
            if (battleManager.currentEnemyStatus != null)
            {
                if (health <= 0) health = 0;
                enemyHealthText.text = "Enemy Health: " + health;
                float enemyHealthRatio = (float)health / battleManager.currentEnemyStatus.Health;
                enemyHealthSlider.value = enemyHealthRatio;
            }
        }

        public void UpdateEnemyBreakPoint(int breakPoint)
        {
            if (battleManager.currentEnemyStatus != null)
            {
                if (breakPoint <= 0) breakPoint = 0;
                enemyBreakPointText.text = "Break Point: " + breakPoint;
                float enemyBreakPointRatio = (float)breakPoint / battleManager.currentEnemyStatus.BreakPoint;
                enemyBreakPointSlider.value = enemyBreakPointRatio;
            }
        }

        // ���̵� �ƿ� ȿ�� ����
        private void StartFadeEffect(GameObject group, bool fadeIn, float duration)
        {
            // �׷� Ȱ��ȭ
            group.SetActive(true);

            Image[] images = group.GetComponentsInChildren<Image>(true);
            foreach (Image image in images)
            {
                float endValue = fadeIn ? 1.0f : 0.0f; // ���̵� ���̸� 1, ���̵� �ƿ��̸� 0
                image.DOFade(endValue, duration).SetUpdate(true);
            }

            // ���̵� �ƿ��� ���, ���̵尡 �Ϸ�Ǹ� �׷��� ��Ȱ��ȭ
            if (!fadeIn)
            {
                DOVirtual.DelayedCall(duration, () => group.SetActive(false));
            }
        }

        private void IsFading()
        {
            if (!isFading)
            {
                return;
            }

            Image[] images = battleWonGroup.GetComponentsInChildren<Image>(true);
            foreach (Image image in images)
            {
                // ���̵� �� �Ǵ� �ƿ��� ���� ���� ���� ����
                currentAlpha += Time.deltaTime / fadeDuration * (isFadeIn ? 1 : -1);
                currentAlpha = Mathf.Clamp01(currentAlpha);

                Color newColor = image.color;
                newColor.a = currentAlpha;
                image.color = newColor;

                // ���̵� �ƿ� �Ϸ� �� �̹��� ��Ȱ��ȭ
                if (!isFadeIn && currentAlpha <= 0)
                {
                    image.gameObject.SetActive(false);
                }
            }

            if (!isFadeIn && currentAlpha <= 0)
            {
                isFading = false;
                // ��� �̹����� ���̵� �ƿ� �Ϸ� �� �׷� ��Ȱ��ȭ
                battleWonGroup.SetActive(false);
                Debug.Log("UIManager: ���̵� �ƿ� �Ϸ� �� �̹��� ��Ȱ��ȭ");
            }

            // ���̵� �� �Ϸ� ��
            if (isFadeIn && currentAlpha >= 1)
            {
                isFading = false;
                // ��� �̹����� ���̵� �� �Ϸ� �� �߰� �۾�
                Debug.Log("UIManager: ���̵� �� �Ϸ�");
            }
        }

        // UI �̹����� ������ �ʱ�ȭ�ϴ� �Լ�
        public void ResetAlpha(Image image)
        {
            if (image != null)
            {
                currentAlpha = isFadeIn ? 1.0f : 0.0f;
                image.color = originalColor; // �ʱ� �������� �ǵ���
            }
        }

        // ��ư ���̶���Ʈ ���
        public void HighlightButton(Button button)
        {
            if (button == null) return;

            currentButton = button;

            // ��ư�� Transform�� �����ɴϴ�
            Transform buttonTransform = button.transform;

            // ��ư ũ�⸦ �����ϴ� �ִϸ��̼��� �����մϴ�
            buttonTransform.DOScale(new Vector3(highlightScale, highlightScale, 1f), highlightDuration)
                .SetLoops(-1, LoopType.Yoyo); // ���� �ݺ�, Yoyo ȿ���� ũ�Ⱑ Ŀ���� �۾����ٸ� �ݺ��մϴ�


        }

        // ���̶���Ʈ�� �����ϴ� ���
        public void StopHighlight(Button button)
        {
            if (button == null) return;

            // ��ư�� Transform�� �����ɴϴ�
            Transform buttonTransform = button.transform;

            // ��ư ũ�� �ִϸ��̼��� �����ϰ� ���� ũ��� �ǵ����ϴ�
            buttonTransform.DOKill(); // �ִϸ��̼��� �����մϴ�
            buttonTransform.localScale = new Vector3(1f, 1f, 1f); // ���� ũ��� �ǵ����ϴ�
        }
    }
}