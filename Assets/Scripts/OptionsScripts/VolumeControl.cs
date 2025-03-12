using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    public TextMeshProUGUI volumePercentage;

    private void Start()
    {
        // Load saved volume setting
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        volumeSlider.value = savedVolume;

        UpdateVolumeText(savedVolume);


        // Add listener to detect slider changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float volume)
    {
        // Find AudioManager and update volume
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.SetVolume(volume);
        }
        UpdateVolumeText(volume);

    }

    void UpdateVolumeText(float volume)
    {
        int percentage = Mathf.RoundToInt(volume * 100);
        volumePercentage.text = percentage + "%";

    }
}
