using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DefenceGameManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text gameOverScore;
    [SerializeField] private Text scoreGoldText;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameStartUI;

    public PlayerLife playerLife;

    int score = 0;
    int life = 5;
    bool isPlaying = false;

    //½Ì±ÛÅæ
    public static DefenceGameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // ¾À ÀüÈ¯µÇ´õ¶óµµ ÆÄ±«µÇÁö ¾Ê°Ô ÇÔ
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        WorldSoundManager.Instance.PlayBGM("DefenceGame Start Menu BGM");
        playerLife = GetComponent<PlayerLife>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();

        CheckGameOver();
    }

    void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int addScore)
    {
        score += addScore;
    }

    public int GetLifeCount()
    {
        return life;
    }

    public void CalculateLife(int value)
    {
        life += value;
    }

    public void GameStart()
    {
        isPlaying = true;
        gameStartUI.SetActive(false);
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(50.0f, 90.0f, 0.0f));
        Cursor.lockState = CursorLockMode.Locked;
        WorldSoundManager.Instance.PlayBGM("DefenceGame Battle BGM");
    }

    void CheckGameOver()
    {
        if (life <= 0)
        {
            isPlaying = false;
            Cursor.lockState = CursorLockMode.None;
            gameOverScore.text = "´Þ¼º Á¡¼ö : " + score.ToString();
            scoreGoldText.text = "È¹µæ °ñµå : " + ((int)(score * 0.5f)).ToString();
            gameOverUI.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    public void ReturnWorldScene()
    {
        PlayerData.Instance.SetGold((int)(score * 0.5f));
        LoadingSceneManager.Instance.StartLoadScene("WorldMap");
    }
}
