using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class WinLostManager : MonoBehaviour
{
    public TMP_Text battleCount;
    public TMP_Text header;
    public Button nextRoundButton;
    private static readonly string saveFilePath = "Assets/Resources/Save/SaveFile.txt";


    private async void Awake()
    {
        AudioManager.GetInstance().PauseMusic();
        battleCount.text = "Final Battle Count: " + PlayerPrefs.GetString("BattleCount", "0");

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

            if (DatabaseManager.IsLoggedIn())
            {
                bool flag = await DatabaseManager.UpdateRank();

                if (flag)
                {
                    AudioManager.GetInstance().PlayRankUpSound();
                    header.text += "You Ranked up!";
                }
            }
        }
    }

    public void NextBattle()
    {
        PlayerPrefs.SetInt("toLoad", 0);

        PlayerPrefs.Save();
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        SceneManager.LoadScene("ChooseAbilities", LoadSceneMode.Single);
    }


    public void ExitBackToMenu()
    {
        PlayerPrefs.SetString("BattleCount", "0");
        PlayerPrefs.SetInt("toLoad", 0);

        PlayerPrefs.Save();
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);  // Load the gameplay scene
    }
}
