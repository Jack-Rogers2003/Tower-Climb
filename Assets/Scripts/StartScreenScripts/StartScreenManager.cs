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
        string usernname = PlayerPrefs.GetString("UserName", "");
        if(usernname != "")
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    // Quit the game
    public void ExitScreen()
    {
        Application.Quit();  // This will close the game
        UnityEditor.EditorApplication.isPlaying = false;  // Stop the game in the editor
    }

   public void EnterButton()
    {
        string enteredText = userInput.text;

        if (enteredText != string.Empty)
        {
            DatabaseManager.CreateNewUser(enteredText);
            PlayerPrefs.SetString("UserName", enteredText);
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
