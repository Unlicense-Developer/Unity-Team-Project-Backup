using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundManager : MonoBehaviour
{
    public static WorldSoundManager Instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    // ���� Ŭ���� �̸����� ã�� ���� ��ųʸ�
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();
    private bool isBattleMode = false;

    void Awake()
    {
        // �̱��� ������ Ȱ���Ͽ� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // ���� ��ųʸ� �ʱ�ȭ
        InitializeAudioDictionaries();
    }

    private void Start()
    {
        // ���� ���� �� �⺻ BGM ���
        PlayBGM("WorldMap");
    }

    // ���� ��ųʸ� �ʱ�ȭ
    private void InitializeAudioDictionaries()
    {
        // SFX ��ųʸ� �ʱ�ȭ
        foreach (AudioClip clip in sfxClips)
        {
            if (clip != null)
            {
                sfxDictionary.Add(clip.name, clip);
            }
        }

        // BGM ��ųʸ� �ʱ�ȭ
        foreach (AudioClip clip in bgmClips)
        {
            if (clip != null)
            {
                bgmDictionary.Add(clip.name, clip);
            }
        }
    }

    // Ư�� �̸��� SFX ���
    public void PlaySFX(string name)
    {
        if (sfxDictionary.TryGetValue(name, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Sound not found: " + name);
        }
    }

    // Ư�� �̸��� BGM ���
    public void PlayBGM(string name)
    {
        if (bgmDictionary.TryGetValue(name, out AudioClip clip))
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
        else
        {
            Debug.LogError("BGM not found: " + name);
        }
    }

    public void StartCityBGM()
    {
        PlayBGM("WorldMap");
    }

    public void StartDungeonBGM()
    {
        PlayBGM("Dungeon");
    }

    // ���� ��� ���� �� ȣ��
    public void StartBattleMode()
    {
        isBattleMode = true;
        PlayBGM("BattleBGM");
    }

    // ���� ��� ���� �� ȣ��
    public void EndBattleMode()
    {
        isBattleMode = false;
        PlayBGM("Dungeon");
    }
}