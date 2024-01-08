using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementBase : MonoBehaviour
{
    [SerializeField] TMP_Text achieve_Text;
    [SerializeField] TMP_Text reward_GoldText;
    [SerializeField] Slider sliderBar;

    GameObject rewardGoldUI;
    GameObject aquiredGoldUI;
    
    int achieve_MaxValue = 0;
    int achieve_Value = 0;
    int rewardGold = 0;
    bool isClear = false;

    // Start is called before the first frame update
    void Start()
    {
        rewardGoldUI = transform.Find("Button_Reward_Coin").gameObject;
        aquiredGoldUI = transform.Find("Button_Acquired").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CheckClearAchievement();
    }

    public void UpdateAchievement()
    {
        achieve_Text.text = achieve_Value.ToString() + " / " + achieve_MaxValue.ToString();
        sliderBar.value = achieve_Value;
    }

    void CheckClearAchievement()
    {
        if( !isClear && achieve_Value == achieve_MaxValue)
        {
            GameClearAchievement();
        }
    }

    public void GameClearAchievement()
    {
        isClear = true;
        achieve_Value = achieve_MaxValue;
        UpdateAchievement();
    }

    public void InitAchievement(int maxValue, int gold)
    {
        achieve_MaxValue = maxValue;
        rewardGold = gold;
        reward_GoldText.text = rewardGold.ToString();
        UpdateAchievement();
    }

    public void SetAchieveValue(int value)
    {
        if (achieve_Value == achieve_MaxValue)
            return;

        achieve_Value += value;
        UpdateAchievement();

        if (achieve_Value == achieve_MaxValue)
            GameClearAchievement();
    }

    public void ClickRewardButton()
    {
        if (!isClear) return;

        rewardGoldUI.SetActive(false);
        aquiredGoldUI.SetActive(true);
    }
}
