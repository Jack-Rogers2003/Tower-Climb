using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;


public class PopulateLeaderboard : MonoBehaviour
{
    public GameObject textPrefab;       // Assign the Text prefab here
    public Transform contentParent;     // Assign Content here
    public ScrollRect scrollRect;       // Assign the ScrollView here

    public void AddLine(string message)
    {
        GameObject newTextObj = Instantiate(textPrefab, contentParent);
        TextMeshProUGUI textComponent = newTextObj.GetComponent<TextMeshProUGUI>();
        textComponent.text = message;

        Canvas.ForceUpdateCanvases();
    }

    public async void Start()
    {
        AddLine("<mark=#FF0000>Welcome to the Leaderboard!</mark>");

        List<(string, string, string)> ranks = await DatabaseManager.ReadData();
        ranks = ranks.OrderByDescending(x => int.Parse(x.Item3)).ToList();

        int position = 1;
        foreach (var pair in ranks)
        {
            if (pair.Item1 == PlayerPrefs.GetString("id"))
            {
                AddLine("<mark=#FF0000>Rank : " + position + " ID: #" + pair.Item1 + " Username: " + pair.Item2 + " Battle Count: " + pair.Item3 + "</mark>");

            }
            else
            {
                AddLine("Rank : " + position + " ID: #" + pair.Item1 + " Username: " + pair.Item2 + " Battle Count: " + pair.Item3);
            }
            position++;
        }



    }
}
