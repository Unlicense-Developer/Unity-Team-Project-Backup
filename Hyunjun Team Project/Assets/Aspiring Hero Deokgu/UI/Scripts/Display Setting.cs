using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplaySetting: MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Button fullScreenBtn;
    [SerializeField] private Button windowedBtn;
    
    List<Resolution> resolutions = new List<Resolution>();
    public FullScreenMode displayMode;
    int resolutionNum;

    // Start is called before the first frame update
    void Start()
    {
        InitUI();
        displayMode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution(1920, 1080, displayMode);
        CheckCurDisplayMode();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void InitUI()
    {
        //resolutions.AddRange(Screen.resolutions);

        foreach (Resolution resolution in Screen.resolutions)
        {
            if( resolution.refreshRate == 60)
            {
                resolutions.Add(resolution);
            }
        }

        resolutionDropdown.options.Clear();

        int optionNum = 0;

        foreach(Resolution resolution in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = resolution.width + " x " + resolution.height + " " + resolution.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if (resolution.width == Screen.width && resolution.height == Screen.height)
                resolutionDropdown.value = optionNum;

            optionNum++;
        }

        resolutionDropdown.RefreshShownValue();
    }

    public void DropdownChangeOption(int num)
    {
        resolutionNum = num;
    }

    public void ChangeDisplayMode(string modeName)
    {
        if( modeName == "FullScreen")
        {
            displayMode = FullScreenMode.FullScreenWindow;
        }
        else if(modeName == "Windowed")
        {
            displayMode = FullScreenMode.Windowed;
        }

        CheckCurDisplayMode();
    }

    public void ChangeSettingButton()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, displayMode);
    }

    public void CheckCurDisplayMode()
    {
        if(displayMode == FullScreenMode.FullScreenWindow)
        {
            fullScreenBtn.GetComponent<Image>().color = new Color(0.67f, 0.44f, 0.22f);
            windowedBtn.GetComponent<Image>().color = Color.white;
        }
        else if (displayMode == FullScreenMode.Windowed)
        {
            windowedBtn.GetComponent<Image>().color = new Color(0.67f, 0.44f, 0.22f);
            fullScreenBtn.GetComponent<Image>().color = Color.white;
        }

    }
}
