using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager1 : MonoBehaviour
{


    public AudioSource bgmPlayer; // 배경음악을 재생할 오디오 소스
    public AudioSource[] sfxPlayer; // 사운드 효과를 재생할 오디오 소스 배열
    public AudioClip[] sfxClip; // 사용할 사운드 효과 클립
    public enum Sfx { Cast, ReelIn, Success, Fail, Catch } // 사운드 효과 유형
    int sfxCursor; // 현재 재생할 사운드 효과의 인덱스


    void Start()
    {
        
    }

    public void SfxPlay(Sfx type)
    {
        switch (type)
        {
            case Sfx.Cast:
                sfxPlayer[sfxCursor].clip = sfxClip[0];
                break;
            case Sfx.ReelIn:
                sfxPlayer[sfxCursor].clip = sfxClip[1];
                break;
            case Sfx.Success:
                sfxPlayer[sfxCursor].clip = sfxClip[2];
                break;
            case Sfx.Fail:
                sfxPlayer[sfxCursor].clip = sfxClip[3];
                break;
            case Sfx.Catch:
                sfxPlayer[sfxCursor].clip = sfxClip[4];
                break;
        }

        sfxPlayer[sfxCursor].Play(); // 선택된 사운드 효과 재생
        sfxCursor = (sfxCursor + 1) % sfxPlayer.Length; // 다음 사운드 효과를 위한 인덱스 업데이트

    }


    void Update()
    {
        
    }
}
