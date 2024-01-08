using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager1 : MonoBehaviour
{


    public AudioSource bgmPlayer; // ��������� ����� ����� �ҽ�
    public AudioSource[] sfxPlayer; // ���� ȿ���� ����� ����� �ҽ� �迭
    public AudioClip[] sfxClip; // ����� ���� ȿ�� Ŭ��
    public enum Sfx { Cast, ReelIn, Success, Fail, Catch } // ���� ȿ�� ����
    int sfxCursor; // ���� ����� ���� ȿ���� �ε���


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

        sfxPlayer[sfxCursor].Play(); // ���õ� ���� ȿ�� ���
        sfxCursor = (sfxCursor + 1) % sfxPlayer.Length; // ���� ���� ȿ���� ���� �ε��� ������Ʈ

    }


    void Update()
    {
        
    }
}
