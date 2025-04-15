using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);  // Load the gameplay scene
    }

    public void ResumeGame()
    {
        SceneManager.LoadScene("BattleScreen", LoadSceneMode.Single);

    }
}
