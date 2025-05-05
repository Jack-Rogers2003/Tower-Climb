using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
    public TMP_InputField userInput;
    public Button logOutButton;
    public Button changeNameButton;

    private void Awake()
    {
        if (!DatabaseManager.IsLoggedIn())
        {
            logOutButton.interactable = false;
            changeNameButton.interactable = false;
        }
        userInput.text = PlayerPrefs.GetString("UserName");
    }


    // Start the game and load the GameScene
    public void CloseOptions()
    {
        Scene scene = SceneManager.GetSceneByName("MainMenu");
        foreach (GameObject rootObj in scene.GetRootGameObjects())
        {
            EventSystem es = rootObj.GetComponentInChildren<EventSystem>(true);
            if (es != null)
                es.enabled = true;
        }

        SceneManager.UnloadSceneAsync("OptionsMenu");
    }

    public void ChangeName()
    {
        string newName = userInput.text;

        if (newName != string.Empty)
        {
            PlayerPrefs.SetString("UserName", newName);
            PlayerPrefs.Save();
            DatabaseManager.UpdateUsername(newName);
        }
    }

    public void LogOut()
    {
        DatabaseManager.LogOut();
        SceneManager.LoadScene("StartingScreen", LoadSceneMode.Single);
    }

}
