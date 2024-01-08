using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private Transform lifeObjects;
    [SerializeField] private Image lifePrefab;
    [SerializeField] private Sprite lifeOn;
    [SerializeField] private Sprite lifeOff;

    List<Image> playerLife;
    Vector2 lifePos = new Vector2(-830.0f, -440.0f);
    float xPosPreset = 110.0f;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayerLife();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreatePlayerLife()
    {
        playerLife = new List<Image>();

        for (int i = 0; i < DefenceGameManager.Instance.GetLifeCount(); i++)
        {
            Image temp = Instantiate(lifePrefab, lifeObjects);
            temp.GetComponent<RectTransform>().anchoredPosition = lifePos + new Vector2(xPosPreset * i, 0);
            temp.GetComponent<RectTransform>().localScale = Vector3.one;
            playerLife.Add(temp);
        }
    }

    public void IncreaseLife()
    {
        playerLife[DefenceGameManager.Instance.GetLifeCount()].sprite = lifeOn;
        DefenceGameManager.Instance.CalculateLife(1);
    }

    public void DecreaseLife()
    {
        DefenceGameManager.Instance.CalculateLife(-1);
        playerLife[DefenceGameManager.Instance.GetLifeCount()].sprite = lifeOff;
    }
}
