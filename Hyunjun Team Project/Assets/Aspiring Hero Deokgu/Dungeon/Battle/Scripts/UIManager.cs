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
        public GameObject battleWonGroup; // 승리 UI
        public GameObject battleLostGroup; // 패배 UI

        public Image tutorial;
        public Image pressF;
        public float fadeDuration = 3.0f; // 페이드 지속 시간 (초)
        private float currentAlpha = 1.0f; // 현재 알파 값 (1.0은 완전 불투명, 0.0은 완전 투명)
        private bool isFadeIn = false; // 페이드 인 여부
        private bool isFading = false; // 페이드 중인지 여부
        private Color originalColor; // 초기 색상을 저장하기 위한 변수

        // 버튼 하이라이트 애니메이션을 위한 설정
        private float highlightScale = 1.2f; // 하이라이트 시 크기 비율
        private float highlightDuration = 0.5f; // 하이라이트 지속 시간
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
        public Button retryButton; // 재도전 버튼

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

        // 전투 시작 효과
        public void BattleStartEffect()
        {
            Debug.Log("UIManager: battleStartImage 이미지 활성화");
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
                // 전투 승리 UI
                StartFadeEffect(battleWonGroup, true, 1.0f);
                Invoke("FadeOutBattleWonGroup", 2.0f);
            }
            else if (battleManager.state == BattleManager.BattleState.Lost)
            {
                // 전투 패배 UI
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
            // null 체크 추가
            if (playerStatus == null || battleManager == null || battleManager.currentEnemyStatus == null)
            {
                Debug.LogError("One or more references are null in UIManager.UpdateUI");
                return; // 필요한 참조 중 하나라도 null이면 함수를 종료합니다.
            }

            playerHealthText.text = "Player Health: " + playerStatus.CurrentHealth;
            enemyHealthText.text = "Enemy Health: " + battleManager.currentEnemyStatus.CurrentHealth;
            enemyBreakPointText.text = "Break Point: " + battleManager.currentEnemyStatus.BreakPoint;
            potionCountText.text = "" + playerStatus.PotionCount;

            // 최대 체력 대비 현재 체력의 비율 계산
            float playerHealthRatio = (float)playerStatus.CurrentHealth / playerStatus.Health;
            float enemyHealthRatio = (float)battleManager.currentEnemyStatus.CurrentHealth / battleManager.currentEnemyStatus.Health;
            float enemyBreakPointRatio = (float)battleManager.currentEnemyStatus.CurrentBreakPoint / battleManager.currentEnemyStatus.BreakPoint;

            // Slider 값 업데이트
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

        // 페이드 아웃 효과 시작
        private void StartFadeEffect(GameObject group, bool fadeIn, float duration)
        {
            // 그룹 활성화
            group.SetActive(true);

            Image[] images = group.GetComponentsInChildren<Image>(true);
            foreach (Image image in images)
            {
                float endValue = fadeIn ? 1.0f : 0.0f; // 페이드 인이면 1, 페이드 아웃이면 0
                image.DOFade(endValue, duration).SetUpdate(true);
            }

            // 페이드 아웃의 경우, 페이드가 완료되면 그룹을 비활성화
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
                // 페이드 인 또는 아웃에 따라 알파 값을 조정
                currentAlpha += Time.deltaTime / fadeDuration * (isFadeIn ? 1 : -1);
                currentAlpha = Mathf.Clamp01(currentAlpha);

                Color newColor = image.color;
                newColor.a = currentAlpha;
                image.color = newColor;

                // 페이드 아웃 완료 시 이미지 비활성화
                if (!isFadeIn && currentAlpha <= 0)
                {
                    image.gameObject.SetActive(false);
                }
            }

            if (!isFadeIn && currentAlpha <= 0)
            {
                isFading = false;
                // 모든 이미지의 페이드 아웃 완료 후 그룹 비활성화
                battleWonGroup.SetActive(false);
                Debug.Log("UIManager: 페이드 아웃 완료 및 이미지 비활성화");
            }

            // 페이드 인 완료 시
            if (isFadeIn && currentAlpha >= 1)
            {
                isFading = false;
                // 모든 이미지의 페이드 인 완료 후 추가 작업
                Debug.Log("UIManager: 페이드 인 완료");
            }
        }

        // UI 이미지의 투명도를 초기화하는 함수
        public void ResetAlpha(Image image)
        {
            if (image != null)
            {
                currentAlpha = isFadeIn ? 1.0f : 0.0f;
                image.color = originalColor; // 초기 색상으로 되돌림
            }
        }

        // 버튼 하이라이트 기능
        public void HighlightButton(Button button)
        {
            if (button == null) return;

            currentButton = button;

            // 버튼의 Transform을 가져옵니다
            Transform buttonTransform = button.transform;

            // 버튼 크기를 변경하는 애니메이션을 적용합니다
            buttonTransform.DOScale(new Vector3(highlightScale, highlightScale, 1f), highlightDuration)
                .SetLoops(-1, LoopType.Yoyo); // 무한 반복, Yoyo 효과로 크기가 커졌다 작아졌다를 반복합니다


        }

        // 하이라이트를 중지하는 기능
        public void StopHighlight(Button button)
        {
            if (button == null) return;

            // 버튼의 Transform을 가져옵니다
            Transform buttonTransform = button.transform;

            // 버튼 크기 애니메이션을 중지하고 원래 크기로 되돌립니다
            buttonTransform.DOKill(); // 애니메이션을 중지합니다
            buttonTransform.localScale = new Vector3(1f, 1f, 1f); // 원래 크기로 되돌립니다
        }
    }
}