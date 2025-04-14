using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LeaderboardEvents : MonoBehaviour
{
    public void ExitLeaderboard()
    {
        SceneManager.UnloadSceneAsync("Leaderboard");
        Scene scene = SceneManager.GetSceneByName("MainMenu");
        foreach (GameObject rootObj in scene.GetRootGameObjects())
        {
            EventSystem es = rootObj.GetComponentInChildren<EventSystem>(true);
            if (es != null)
                es.enabled = true;
        }

    }
}
