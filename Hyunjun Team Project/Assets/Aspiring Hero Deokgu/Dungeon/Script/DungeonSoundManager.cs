using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class DungeonSoundManager : MonoBehaviour
    {
        public static DungeonSoundManager Instance;

        public AudioSource bgmSource;
        public AudioSource sfxSource;

        public AudioClip normalBGM;
        public AudioClip battleBGM;

        // 오디오 클립 배열
        public AudioClip[] sfxClips;

        private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();
        private bool isBattleMode = false;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            InitializeSFXDictionary();
        }

        private void Start()
        {
            PlayBGM(false);
        }

        private void InitializeSFXDictionary()
        {
            foreach (AudioClip clip in sfxClips)
            {
                if (clip != null)
                {
                    sfxDictionary.Add(clip.name, clip);
                }
            }
        }

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

        public void StartBattleMode()
        {
            isBattleMode = true;
            //bgmSource.Pause();
            PlayBGM(true);
        }

        public void EndBattleMode()
        {
            isBattleMode = false;
            PlayBGM(false);
            //bgmSource.UnPause();
        }

        public void PlayBGM(bool isBattle)
        {
            bgmSource.clip = isBattle ? battleBGM : normalBGM;
            bgmSource.Play();
        }
    }
}