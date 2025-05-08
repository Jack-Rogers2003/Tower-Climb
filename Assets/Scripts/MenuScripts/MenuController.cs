using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public Button leaderboardButton;
    public Button loadGameButton;
    private static readonly string saveFilePath = "Assets/Resources/Save/SaveFile.txt";
    private readonly AudioManager audioManager = AudioManager.GetInstance();



    public void Start()
    {
        audioManager.PlayMenuMusic();
        while (leaderboardButton == null)
        {
            return;  // Wait until the button is not null
        }
        if (!DatabaseManager.IsLoggedIn())
        {
            leaderboardButton.interactable = false;
        }
        while (loadGameButton == null)
        {
            return;  
        }
        if (!File.Exists(saveFilePath))
        {
            loadGameButton.interactable = false;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("ChooseAbilities", LoadSceneMode.Single);  // Load the gameplay scene
    }

    public void LoadOptions()
    {
        EventSystem.current.enabled = false;
        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
    }

    public void LoadLeaderboard()
    {
        EventSystem.current.enabled = false;
        SceneManager.LoadScene("Leaderboard", LoadSceneMode.Additive);
    }


    // Quit the game
    public void ExitGame()
    {
        Application.Quit();  // This will close the game
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        // Stop the game in the editor
    }

    public void ActivateEventSystem()
    {
        EventSystem.current.enabled = true;
    }

    public void LoadGame()
    {
        PlayerPrefs.SetInt("toLoad", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("BattleScreen", LoadSceneMode.Single);
    }

    public void Credit()
    {
        System.Diagnostics.Process.Start(Path.Combine(Application.streamingAssetsPath, "credit.txt"));
    }
}
