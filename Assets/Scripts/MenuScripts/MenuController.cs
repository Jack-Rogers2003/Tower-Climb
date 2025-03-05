using UnityEngine;
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
        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);  // Load the options scene
    }

    // Quit the game
    public void ExitGame()
    {
        Application.Quit();  // This will close the game
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Stop the game in the editor
#endif
    }
}
