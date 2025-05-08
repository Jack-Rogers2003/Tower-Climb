using System;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public AudioSource menuMusic;
    public AudioSource battleMusic;
    private AudioSource currentMusic;
    public AudioClip rankUp;
    public AudioClip swordAttack;
    public AudioClip dragonRoar;
    public AudioClip healSound;

    private const string VolumeKey = "MusicVolume"; // Key for saving volume

    void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Keep it across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentMusic = menuMusic;

        PlayMusic();
    }

    internal void PlayRankUpSound()
    {
        currentMusic.PlayOneShot(rankUp);
    }

    internal void PlayHealSound()
    {
        currentMusic.PlayOneShot(healSound);
    }

    internal void PlaySwordAttack()
    {
        currentMusic.PlayOneShot(swordAttack);
    }

    internal void PlayDragonRoar()
    {
        currentMusic.PlayOneShot(dragonRoar);
    }

    private void PlayMusic()
    {
        currentMusic.volume = PlayerPrefs.GetFloat(VolumeKey, 0.5f);
        currentMusic.loop = true;
        currentMusic.Play();
    }

    // Method to set volume from Options Menu
    public void SetVolume(float volume)
    {
        menuMusic.volume = volume;
        PlayerPrefs.SetFloat(VolumeKey, volume);  // Save volume setting
        PlayerPrefs.Save();
    }

    internal static AudioManager GetInstance()
    {
        return instance;
    }

    internal void StopMusic()
    {
        instance.StopMusic();
        instance = null;
    }

    internal void PauseMusic()
    {
        currentMusic.Pause();
    }

    internal void ResumeMusic()
    {
        currentMusic.UnPause();
    }

    internal void PlayBattleMusic()
    {
        if (!currentMusic.isPlaying && currentMusic == battleMusic)
        {
            ResumeMusic();
        }
        else
        {
            currentMusic.Stop();
            currentMusic = battleMusic;
            PlayMusic();
        }
    }

    internal void PlayMenuMusic()
    {
        if (currentMusic != menuMusic)
        {
            currentMusic.Stop();
            // Load the saved volume setting
            currentMusic = menuMusic;
            PlayMusic();
        }
    }
}
