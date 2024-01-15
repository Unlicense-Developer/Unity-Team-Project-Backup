using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;

public class ButtonManager : MonoBehaviour
{
    private int activeButtons = 0;

    public void ButtonActivated()
    {
        activeButtons++;
        WorldSoundManager.Instance.PlaySFX("Rune");

        Debug.Log(activeButtons);

        if (activeButtons >= 4)
        {
            BossActive.instance.DeviceActivated();

        }
    }
}
