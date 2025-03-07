using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    public TextMeshProUGUI volumePercentage;
   // Updated to match exact name

    void Start()
    {
        // Load saved volume and set slider value
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        volumeSlider.value = savedVolume;

        // Update text
        UpdateVolumeText(savedVolume);

        // Add listener for changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float volume)
    {
        // Find the MusicManager and update volume
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        if (musicManager != null)
        {
            musicManager.SetVolume(volume);
        }

        // Save volume setting
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();

        // Update percentage text
        UpdateVolumeText(volume);
    }

    void UpdateVolumeText(float volume)
    {
        int percentage = Mathf.RoundToInt(volume * 100);
        volumePercentage.text = percentage + "%";
     
    }
}
