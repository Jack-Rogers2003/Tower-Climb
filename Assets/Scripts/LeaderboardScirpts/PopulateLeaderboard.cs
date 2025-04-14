using UnityEngine;
using UnityEngine.UI;
using TMPro;


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

    void Start()
    {
        // Example usage
        AddLine("<mark=#FF0000>Welcome to the dynamic Scroll View!</mark>");
        AddLine("Line 2: You can add text at runtime.");
        AddLine("Line 3: This is added automatically.");
        AddLine("Line 4: This is added automatically.");
        AddLine("Line 5: This is added automatically.");
        AddLine("Line 6: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");
        AddLine("Line 7: This is added automatically.");



    }
}
