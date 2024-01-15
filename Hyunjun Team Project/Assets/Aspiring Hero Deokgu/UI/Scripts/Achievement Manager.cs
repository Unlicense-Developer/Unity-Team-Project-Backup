using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private AchievementBase achieve_VeganKnight;
    [SerializeField] private AchievementBase achieve_Alchemy;
    [SerializeField] private AchievementBase achieve_Defence;
    [SerializeField] private AchievementBase achieve_GrillingMeat;
    [SerializeField] private AchievementBase achieve_Dungeon;

    [SerializeField] private GameObject achievement_UI;
    

    public static AchievementManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(this.gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        achieve_VeganKnight.InitAchievement(1, 1000);
        achieve_Alchemy.InitAchievement(1, 2000);
        achieve_Defence.InitAchievement(3, 5000);
        achieve_GrillingMeat.InitAchievement(1, 1500);
        achieve_Dungeon.InitAchievement(1, 10000);
    }
        
    // Update is called once per frame
    void Update()
    {
     
        
    }

    public void SetAchieveValue(string gameName, int value)
    {
        if (gameName == "VeganKnight")
        {
            achieve_VeganKnight.SetAchieveValue(value);
        }
        else if (gameName == "Alchemy")
        {
            achieve_Alchemy.SetAchieveValue(value);
        }
        else if (gameName == "GrillingMeat")
        {
            achieve_GrillingMeat.SetAchieveValue(value);
        }
        else if (gameName == "Defence")
        {
            achieve_Defence.SetAchieveValue(value);
        }
        else if (gameName == "Dungeon")
        {
            achieve_Dungeon.SetAchieveValue(value);
        }

    }

    public void ActivateUI()
    {
        achievement_UI.SetActive(true);
    }
}
