using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public TMP_Text nextRank;
    public TextMeshProUGUI header;

    private void Awake()
    {
        SetNextRank();
    }

    public void ExitGame()
    {
        PlayerPrefs.SetString("BattleCount", "0");
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); 
    }

    public void ResumeGame()
    {
        PlayerPrefs.SetInt("toLoad", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("BattleScreen", LoadSceneMode.Single);

    }

    public async void SetNextRank()
    {
        if (DatabaseManager.IsLoggedIn())
        {
            List<(string, string)> ranks = await DatabaseManager.GetBattles();
            ranks = ranks.OrderByDescending(x => int.Parse(x.Item2)).ToList();
            string id = PlayerPrefs.GetString("id");
            foreach (var pair in ranks)
            {
                int index = ranks.IndexOf(pair);
                if (pair.Item1 == id)
                {
                    if (index == 0)
                    {
                        nextRank.text = "You are the highest rank!";
                    }
                    else
                    {
                        var higherPair = ranks[index - 1];
                        nextRank.text = "To next rank: " + (int.Parse(higherPair.Item2) - PlayerPrefs.GetInt("BattleCount", 0) + 1);
                    }
                }
            }
        } else
        {
            header.text = "";
        }
    }
}
