using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private AchievementBase achieve_Defence;
    [SerializeField] private AchievementBase achieve_Alchemy;
    [SerializeField] private AchievementBase achieve_GrillingMeat;
    [SerializeField] private AchievementBase achieve_VeganKnight;
    [SerializeField] private AchievementBase achieve_Delivery;
    [SerializeField] private AchievementBase achieve_Fishing;
    

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
        achieve_Defence.InitAchievement(3, 3000);
        achieve_Alchemy.InitAchievement(1, 2000);
        achieve_GrillingMeat.InitAchievement(1, 1500);
        achieve_VeganKnight.InitAchievement(1, 1000);
        achieve_Delivery.InitAchievement(500, 2000);
        achieve_Fishing.InitAchievement(1, 3000);
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
        else if (gameName == "Delivery")
        {
            achieve_Delivery.SetAchieveValue(value);
        }
        else if (gameName == "Fishing")
        {
            achieve_Fishing.SetAchieveValue(value);
        }

    }
}
