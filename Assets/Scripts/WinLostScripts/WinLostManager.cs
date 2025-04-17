using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class WinLostManager : MonoBehaviour
{
    public TMP_Text battleCount;
    public TMP_Text header;


    private void Awake()
    {

        if (PlayerPrefs.GetInt("HasWon", 0) == 0)
        {
            header.text = "You Lost!";
            battleCount.text = "Final Battle Count: " + PlayerPrefs.GetString("BattleCount", "0");

        }
        else
        {
            header.text = "You Won!";

        }
        battleCount.text = "Final Battle Count: " + PlayerPrefs.GetString("BattleCount", "0");
        DatabaseManager.UpdateRank();
    }


    public void ExitBackToMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);  // Load the gameplay scene
    }
}
