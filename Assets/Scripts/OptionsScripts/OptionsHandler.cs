using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsHandler : MonoBehaviour
{
    // Start the game and load the GameScene
    public void closeOptions()
    {
        SceneManager.UnloadSceneAsync("OptionsMenu");
    }
}
