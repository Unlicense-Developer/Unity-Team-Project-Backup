using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VeganNinjaManager : MonoBehaviour
{
    public static VeganNinjaManager Instance { get; private set; }

    [SerializeField] private Blade blade;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text gameOverScoreText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Image awardImage;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private List<AudioClip> sliceSounds;
    [SerializeField] private AudioClip gameoverSound;

    AudioSource sliceSound;

    public bool isSliceFruit = false;
    bool isPlaying = false;
    int score;
    int playerLife = 5;

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
            awardImage.sprite = ItemDataManager.instance.GetItem("Apple").icon;
            PlayerData.instance.AddItemData("Apple");
        }
        else if (score >= 200)
        {
            awardImage.sprite = ItemDataManager.instance.GetItem("Watermelon").icon;
            PlayerData.instance.AddItemData("Watermelon");
        }
        else if (score >= 100)
        {
            awardImage.sprite = ItemDataManager.instance.GetItem("Avocado").icon;
            PlayerData.instance.AddItemData("Avocado");
        }
        else if (score >= 30)
        {
            awardImage.sprite = ItemDataManager.instance.GetItem("Grape").icon;
            PlayerData.instance.AddItemData("Grape");
        }
        else
        {
            awardImage.sprite = ItemDataManager.instance.GetItem("Orange").icon;
            PlayerData.instance.AddItemData("Orange");
        }
    }

    public void ReturnWorldScene()
    {
        SceneManager.LoadScene("WorldMap");
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

        //NewGame();

        //elapsed = 0f;

        //// Fade back in
        //while (elapsed < duration)
        //{
        //    float t = Mathf.Clamp01(elapsed / duration);
        //    fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

        //    elapsed += Time.unscaledDeltaTime;

        //    yield return null;
        //}
    }

}
