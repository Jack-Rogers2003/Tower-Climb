using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OptionsHandler : MonoBehaviour
{
    // Start the game and load the GameScene
    public void closeOptions()
    {
        StartCoroutine(CloseOptionsRoutine());

        Scene scene = SceneManager.GetSceneByName("MainMenu");
        foreach (GameObject rootObj in scene.GetRootGameObjects())
        {
            EventSystem es = rootObj.GetComponentInChildren<EventSystem>(true);
            if (es != null)
                es.enabled = true;
        }
    }

    private IEnumerator CloseOptionsRoutine()
    {
        // Unload the overlay scene
        yield return SceneManager.UnloadSceneAsync("OptionsMenu");

        // Wait one frame to let the current click release
        yield return null;
    }
}
