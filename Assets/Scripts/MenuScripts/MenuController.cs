using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start the game and load the GameScene
    public void StartGame()
    {
        SceneManager.LoadScene("BattleScreen", LoadSceneMode.Single);  // Load the gameplay scene
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
}
