using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OptionsHandler : MonoBehaviour
{
    public TMP_InputField userInput;

    private void Awake()
    {
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
            DatabaseManager.UpdateUsername(newName);
        }
    }

}
