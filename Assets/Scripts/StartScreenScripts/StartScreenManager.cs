using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    public TMP_InputField userInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DatabaseManager.Initialize();
        SceneManager.LoadScene("AudioManagerService", LoadSceneMode.Additive);

        string usernname = PlayerPrefs.GetString("UserName", string.Empty);
        if(usernname != string.Empty)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    // Quit the game
    public void ExitScreen()
    {
        Application.Quit();  // This will close the game
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        // Stop the game in the editor
    }

   public void EnterButton()
    {
        string enteredText = userInput.text;

        if (enteredText != string.Empty)
        {
            DatabaseManager.CreateNewUser(enteredText);
            PlayerPrefs.SetString("UserName", enteredText);
            PlayerPrefs.Save();
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
