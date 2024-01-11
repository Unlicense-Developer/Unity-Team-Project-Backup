using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VeganNinjaManager : MonoBehaviour
{
    public static VeganNinjaManager Instance { get; private set; }

    [SerializeField] private Blade blade;
    [SerializeField] private SpawnManager spawnManager;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text gameOverScoreText;

    [SerializeField] private Image fadeImage;
    [SerializeField] private Image awardImage;

    [SerializeField] private GameObject gameStartUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject useItemPanel;

    [SerializeField] private List<AudioClip> sliceSounds;
    [SerializeField] private AudioClip gameoverSound;

    AudioSource sliceSound;

    public bool isImmune = false;
    public bool isSliceFruit = false;
    bool isPlaying = false;
    int score;

    public int Score => score;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start()
    {
        NewGame();
        sliceSound = GetComponent<AudioSource>();

        gameStartUI.SetActive(true);
        WorldSoundManager.Instance.PlayBGM("VeganKnight Start Menu BGM");
    }

    private void Update()
    {
        scoreText.text = score.ToString();
    }

    private void NewGame()
    {
        Time.timeScale = 1f;

        ClearScene();

        blade.enabled = true;
        spawnManager.enabled = true;

        score = 0;
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    public void SetImmuneState()
    {
        isImmune = true;
        useItemPanel.SetActive(true);
        StartCoroutine(ReturnFromImmune());
    }

    IEnumerator ReturnFromImmune()
    {
        yield return new WaitForSeconds(2.0f);
        useItemPanel.SetActive(false);
        isImmune = false;
    }

    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach (Fruit fruit in fruits) {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach (Bomb bomb in bombs) {
            Destroy(bomb.gameObject);
        }
    }

    public void IncreaseScore(int points)
    {
        sliceSound.PlayOneShot(sliceSounds[Random.Range(0, 3)]);

        score += points;
        scoreText.text = score.ToString();

        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }
    }

    public void Explode()
    {
        blade.enabled = false;
        spawnManager.enabled = false;

        StartCoroutine(ExplodeSequence());
    }

    void CheckScoreAward()
    {
        if( score >= 500)
        {
            awardImage.sprite = ItemDataManager.Instance.GetItem("Apple").icon;
            PlayerData.Instance.AddItemData("Apple");
        }
        else if (score >= 200)
        {
            awardImage.sprite = ItemDataManager.Instance.GetItem("Watermelon").icon;
            PlayerData.Instance.AddItemData("Watermelon");
        }
        else if (score >= 100)
        {
            awardImage.sprite = ItemDataManager.Instance.GetItem("Avocado").icon;
            PlayerData.Instance.AddItemData("Avocado");
        }
        else if (score >= 30)
        {
            awardImage.sprite = ItemDataManager.Instance.GetItem("Grape").icon;
            PlayerData.Instance.AddItemData("Grape");
        }
        else
        {
            awardImage.sprite = ItemDataManager.Instance.GetItem("Orange").icon;
            PlayerData.Instance.AddItemData("Orange");
        }
    }

    public void ReturnWorldScene()
    {
        if( score != 0)
        {
            AchievementManager.Instance.SetAchieveValue("VeganKnight", 1);
        }

        LoadingSceneManager.Instance.StartLoadScene("WorldMap");
        Time.timeScale = 1.0f;
    }

    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        // Fade to white
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(2.0f);

        sliceSound.clip = gameoverSound;
        sliceSound.Play();
        CheckScoreAward();
        gameOverScoreText.text = "달성 점수 : " + score.ToString();
        gameOverUI.SetActive(true);
    }

}
