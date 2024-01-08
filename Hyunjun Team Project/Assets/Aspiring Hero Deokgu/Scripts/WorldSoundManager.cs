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

    // 사운드 클립을 이름으로 찾기 위한 딕셔너리
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();
    private bool isBattleMode = false;

    void Awake()
    {
        // 싱글톤 패턴을 활용하여 인스턴스 관리
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // 사운드 딕셔너리 초기화
        InitializeAudioDictionaries();
    }

    private void Start()
    {
        // 게임 시작 시 기본 BGM 재생
        PlayBGM("WorldMap");
    }

    // 사운드 딕셔너리 초기화
    private void InitializeAudioDictionaries()
    {
        // SFX 딕셔너리 초기화
        foreach (AudioClip clip in sfxClips)
        {
            if (clip != null)
            {
                sfxDictionary.Add(clip.name, clip);
            }
        }

        // BGM 딕셔너리 초기화
        foreach (AudioClip clip in bgmClips)
        {
            if (clip != null)
            {
                bgmDictionary.Add(clip.name, clip);
            }
        }
    }

    // 특정 이름의 SFX 재생
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

    // 특정 이름의 BGM 재생
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

    // 전투 모드 시작 시 호출
    public void StartBattleMode()
    {
        isBattleMode = true;
        PlayBGM("BattleBGM");
    }

    // 전투 모드 종료 시 호출
    public void EndBattleMode()
    {
        isBattleMode = false;
        PlayBGM("Dungeon");
    }
}