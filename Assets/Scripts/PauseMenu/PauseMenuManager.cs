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
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); 
    }

    public void ResumeGame()
    {
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
                        nextRank.text = "To next rank: " + (int.Parse(higherPair.Item2) - PlayerPrefs.GetInt("CurrentBattleCount", 0));
                    }
                }
            }
        } else
        {
            header.text = "";
        }
    }
}
