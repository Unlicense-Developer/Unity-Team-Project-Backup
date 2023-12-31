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

    int orcScore = 0;
    int score = 0;
    int life = 5;
    bool isPlaying = false;

    //�̱���
    public static DefenceGameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // �� ��ȯ�Ǵ��� �ı����� �ʰ� ��
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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
    }

    void CheckGameOver()
    {
        if (life <= 0 && isPlaying)
        {
            isPlaying = false;
            Cursor.lockState = CursorLockMode.None;
            gameOverScore.text = "�޼� ���� : " + score.ToString();
            scoreGoldText.text = "ȹ�� ��� : " + ((int)(score * 0.5f)).ToString();
            gameOverUI.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    public void ReturnWorldScene()
    {
        PlayerData.instance.SetGold((int)(score * 0.5f));
        LoadingSceneManager.Instance.StartLoadScene("WorldMap");
        Time.timeScale = 1.0f;
    }
}
