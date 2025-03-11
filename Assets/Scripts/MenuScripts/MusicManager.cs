using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    private AudioSource audioSource;
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

        // Get or Add AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Load the saved volume setting
        audioSource.volume = PlayerPrefs.GetFloat(VolumeKey, 0.5f);
        audioSource.loop = true;
        audioSource.Play();
    }

    // Method to set volume from Options Menu
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(VolumeKey, volume);  // Save volume setting
        PlayerPrefs.Save();
    }
}
