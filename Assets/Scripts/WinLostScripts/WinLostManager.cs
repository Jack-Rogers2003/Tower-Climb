using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class WinLostManager : MonoBehaviour
{
    public TMP_Text battleCount;
    public TMP_Text header;
    public Button nextRoundButton;


    private void Awake()
    {
        if (PlayerPrefs.GetInt("HasWon", 0) == 0)
        {
            header.text = "You Lost!";
            nextRoundButton.interactable = false;
            nextRoundButton.gameObject.SetActive(false);

        }
        else
        {
            header.text = "You Won!";
            nextRoundButton.interactable = true;
            nextRoundButton.gameObject.SetActive(true);

        }
        battleCount.text = "Final Battle Count: " + PlayerPrefs.GetString("BattleCount", "0");
        DatabaseManager.UpdateRank();
    }

    public void NextBattle()
    {
        SceneManager.LoadScene("ChooseAbilities", LoadSceneMode.Single);
    }


    public void ExitBackToMenu()
    {
        PlayerPrefs.SetString("BattleCount", "0");

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);  // Load the gameplay scene
    }
}
