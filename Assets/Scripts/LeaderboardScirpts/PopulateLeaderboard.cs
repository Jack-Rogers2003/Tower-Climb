using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Firebase.Database;


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

        List<(string, string)> list = await DatabaseManager.ReadData();

        foreach (var pair in list)
        {
            if (pair.Item1 == PlayerPrefs.GetString("id"))
            {
                AddLine("<mark=#FF0000>ID: #" + pair.Item1 + " Username: " + pair.Item2 + "</mark>");

            }
            else
            {
                AddLine("ID: #" + pair.Item1 + " Username: " + pair.Item2);

            }
        }



    }
}
