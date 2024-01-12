using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // 효과음 이름
    public AudioClip clip; // 효과음 클립
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] Sound[] sfx; // 효과음을 담을 배열
    [SerializeField] AudioSource[] sfxPlayer; // 효과음을 재생할 오디오 소스 배열


    private void Awake()
    {
        if (instance = null) instance = this;

        DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
    }

    public void PlaySFX(string p_sfxName)


    {   // 입력받은 이름의 효과음을 찾아서 재생
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == sfx[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {
                    // SfxPlayer에서 재생 중이지 않은 Audio Source를 발견했다면 
                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = sfx[i].clip;
                        sfxPlayer[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 오디오 플레이어가 재생중입니다.");
                return;
            }
        }
        Debug.Log(p_sfxName + " 이름의 효과음이 없습니다.");
        return;
    }
}
